import 'package:flutter/material.dart';
import '../../core/services/auth_service.dart';
import '../../core/services/recipe_service.dart';
import '../../core/services/meal_plan_service.dart';

class RecipeDetailPage extends StatefulWidget {
  final int recipeId;

  const RecipeDetailPage({super.key, required this.recipeId});

  @override
  State<RecipeDetailPage> createState() => _RecipeDetailPageState();
}

class _RecipeDetailPageState extends State<RecipeDetailPage> {
  late AuthService _authService;
  late RecipeService _recipeService;
  late MealPlanService _mealPlanService;
  dynamic _recipe;
  bool _isLoading = true;
  int _selectedServings = 1;

  @override
  void initState() {
    super.initState();
    _authService = AuthService();
    _init();
  }

  Future<void> _init() async {
    await _authService.init();
    _recipeService = RecipeService(_authService);
    _mealPlanService = MealPlanService(_authService);
    _loadRecipe();
  }

  Future<void> _loadRecipe() async {
    setState(() => _isLoading = true);
    final recipe = await _recipeService.getRecipeById(widget.recipeId);
    setState(() {
      _recipe = recipe;
      _isLoading = false;
    });
  }

  Future<void> _addToMealPlan() async {
    final success = await _mealPlanService.addMealPlan(
      widget.recipeId,
      DateTime.now(),
      _selectedServings,
    );

    if (success) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Đã thêm vào kế hoạch bữa ăn')),
        );
        Navigator.pop(context);
      }
    } else {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Thêm vào kế hoạch thất bại')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Chi tiết công thức'),
        backgroundColor: Colors.green,
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _recipe == null
              ? const Center(child: Text('Không tìm thấy công thức'))
              : SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // Image
                      if (_recipe['imageUrl'] != null)
                        Image.network(
                          _recipe['imageUrl'],
                          height: 250,
                          width: double.infinity,
                          fit: BoxFit.cover,
                        )
                      else
                        Container(
                          height: 250,
                          width: double.infinity,
                          color: Colors.grey[300],
                          child: const Icon(Icons.restaurant_menu, size: 100),
                        ),
                      Padding(
                        padding: const EdgeInsets.all(16),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            // Title
                            Text(
                              _recipe['recipeName'] ?? 'Tên công thức',
                              style: const TextStyle(
                                fontSize: 24,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            const SizedBox(height: 8),
                            // Description
                            Text(
                              _recipe['description'] ?? 'Mô tả',
                              style: const TextStyle(
                                fontSize: 14,
                                color: Colors.grey,
                              ),
                            ),
                            const SizedBox(height: 16),

                            // Stats Row
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceAround,
                              children: [
                                _buildStatCard(
                                  Icons.local_fire_department,
                                  '${_recipe['calories'] ?? 0}',
                                  'Calo',
                                ),
                                _buildStatCard(
                                  Icons.timer,
                                  '${_recipe['preparationTime'] ?? 0}',
                                  'phút',
                                ),
                                _buildStatCard(
                                  Icons.people,
                                  '${_recipe['servings'] ?? 0}',
                                  'khẩu phần',
                                ),
                              ],
                            ),
                            const SizedBox(height: 24),

                            // Ingredients
                            const Text(
                              'Nguyên liệu',
                              style: TextStyle(
                                fontSize: 18,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            const SizedBox(height: 12),
                            if (_recipe['recipeIngredients'] != null)
                              Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: (_recipe['recipeIngredients']
                                        as List<dynamic>)
                                    .map<Widget>((ingredient) {
                                  return Padding(
                                    padding: const EdgeInsets.symmetric(
                                      vertical: 4,
                                    ),
                                    child: Row(
                                      children: [
                                        const Icon(
                                          Icons.circle,
                                          size: 6,
                                        ),
                                        const SizedBox(width: 12),
                                        Expanded(
                                          child: Text(
                                            '${ingredient['quantity']} ${ingredient['unit']} ${ingredient['ingredientName']}',
                                          ),
                                        ),
                                      ],
                                    ),
                                  );
                                }).toList(),
                              ),
                            const SizedBox(height: 24),

                            // Steps
                            const Text(
                              'Các bước',
                              style: TextStyle(
                                fontSize: 18,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            const SizedBox(height: 12),
                            if (_recipe['recipeSteps'] != null)
                              Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: List.generate(
                                  (_recipe['recipeSteps'] as List<dynamic>)
                                      .length,
                                  (index) {
                                    final step = _recipe['recipeSteps'][index];
                                    return Padding(
                                      padding: const EdgeInsets.symmetric(
                                        vertical: 8,
                                      ),
                                      child: Row(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Container(
                                            width: 32,
                                            height: 32,
                                            decoration: BoxDecoration(
                                              color: Colors.green,
                                              borderRadius:
                                                  BorderRadius.circular(
                                                16,
                                              ),
                                            ),
                                            child: Center(
                                              child: Text(
                                                '${index + 1}',
                                                style: const TextStyle(
                                                  color: Colors.white,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                              ),
                                            ),
                                          ),
                                          const SizedBox(width: 12),
                                          Expanded(
                                            child: Text(
                                              step['stepDescription'] ?? '',
                                              style: const TextStyle(
                                                fontSize: 14,
                                              ),
                                            ),
                                          ),
                                        ],
                                      ),
                                    );
                                  },
                                ),
                              ),
                            const SizedBox(height: 24),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
      bottomNavigationBar: _recipe != null
          ? Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text('Khẩu phần:'),
                      Row(
                        children: [
                          IconButton(
                            onPressed: () {
                              if (_selectedServings > 1) {
                                setState(
                                  () => _selectedServings--,
                                );
                              }
                            },
                            icon: const Icon(Icons.remove),
                          ),
                          Text(_selectedServings.toString()),
                          IconButton(
                            onPressed: () {
                              setState(() => _selectedServings++);
                            },
                            icon: const Icon(Icons.add),
                          ),
                        ],
                      ),
                    ],
                  ),
                  SizedBox(
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: _addToMealPlan,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.green,
                        padding: const EdgeInsets.symmetric(vertical: 12),
                      ),
                      child: const Text('Thêm vào kế hoạch bữa ăn'),
                    ),
                  ),
                ],
              ),
            )
          : null,
    );
  }

  Widget _buildStatCard(IconData icon, String value, String label) {
    return Column(
      children: [
        Icon(icon, color: Colors.green),
        const SizedBox(height: 8),
        Text(
          value,
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
        Text(
          label,
          style: const TextStyle(fontSize: 12, color: Colors.grey),
        ),
      ],
    );
  }
}
