import 'package:flutter/material.dart';
import '../../core/services/auth_service.dart';
import '../../core/services/recipe_service.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  late AuthService _authService;
  late RecipeService _recipeService;
  List<dynamic> _recipes = [];
  bool _isLoading = true;
  int _selectedTab = 0; // 0: recipes, 1: meal plan, 2: shopping list

  @override
  void initState() {
    super.initState();
    _authService = AuthService();
    _init();
  }

  Future<void> _init() async {
    await _authService.init();
    _recipeService = RecipeService(_authService);
    _loadRecipes();
  }

  Future<void> _loadRecipes() async {
    setState(() => _isLoading = true);
    final recipes = await _recipeService.getAllRecipes();
    setState(() {
      _recipes = recipes;
      _isLoading = false;
    });
  }

  Future<void> _logout() async {
    await _authService.logout();
    if (mounted) {
      Navigator.of(context).pushReplacementNamed('/login');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('BepTroLy'),
        backgroundColor: Colors.green,
        elevation: 0,
        actions: [
          PopupMenuButton(
            itemBuilder: (context) => [
              PopupMenuItem(
                child: const Text('Đăng xuất'),
                onTap: _logout,
              ),
            ],
          ),
        ],
      ),
      body: _selectedTab == 0
          ? _buildRecipesTab()
          : _selectedTab == 1
              ? _buildMealPlanTab()
              : _buildShoppingListTab(),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedTab,
        onTap: (index) => setState(() => _selectedTab = index),
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.restaurant_menu),
            label: 'Công thức',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.calendar_today),
            label: 'Kế hoạch',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.shopping_cart),
            label: 'Mua sắm',
          ),
        ],
      ),
    );
  }

  Widget _buildRecipesTab() {
    return RefreshIndicator(
      onRefresh: _loadRecipes,
      child: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _recipes.isEmpty
              ? const Center(child: Text('Không có công thức nào'))
              : ListView.builder(
                  itemCount: _recipes.length,
                  itemBuilder: (context, index) {
                    final recipe = _recipes[index];
                    return RecipeCard(
                      recipe: recipe,
                      onTap: () {
                        Navigator.of(context).pushNamed(
                          '/recipe-detail',
                          arguments: recipe['recipeId'],
                        );
                      },
                    );
                  },
                ),
    );
  }

  Widget _buildMealPlanTab() {
    return const Center(
      child: Text('Màn hình Kế hoạch bữa ăn'),
    );
  }

  Widget _buildShoppingListTab() {
    return const Center(
      child: Text('Màn hình Danh sách mua sắm'),
    );
  }
}

class RecipeCard extends StatelessWidget {
  final dynamic recipe;
  final VoidCallback onTap;

  const RecipeCard({
    super.key,
    required this.recipe,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Card(
        margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Recipe Image
            if (recipe['imageUrl'] != null)
              Image.network(
                recipe['imageUrl'],
                height: 200,
                width: double.infinity,
                fit: BoxFit.cover,
              )
            else
              Container(
                height: 200,
                width: double.infinity,
                color: Colors.grey[300],
                child: const Icon(Icons.restaurant_menu, size: 80),
              ),
            Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    recipe['recipeName'] ?? 'Tên công thức',
                    style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Text(
                    recipe['description'] ?? 'Mô tả',
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                    style: const TextStyle(color: Colors.grey),
                  ),
                  const SizedBox(height: 12),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Row(
                        children: [
                          const Icon(Icons.local_fire_department, size: 16),
                          const SizedBox(width: 4),
                          Text('${recipe['calories'] ?? 0} cal'),
                        ],
                      ),
                      Text('${recipe['servings'] ?? 0} khẩu phần'),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
