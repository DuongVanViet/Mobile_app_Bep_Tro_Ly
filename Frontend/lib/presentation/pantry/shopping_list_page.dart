import 'package:flutter/material.dart';
import '../../core/services/auth_service.dart';
import '../../core/services/shopping_list_service.dart';

class ShoppingListPage extends StatefulWidget {
  const ShoppingListPage({super.key});

  @override
  State<ShoppingListPage> createState() => _ShoppingListPageState();
}

class _ShoppingListPageState extends State<ShoppingListPage> {
  late AuthService _authService;
  late ShoppingListService _shoppingListService;
  List<dynamic> _shoppingLists = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _authService = AuthService();
    _init();
  }

  Future<void> _init() async {
    await _authService.init();
    _shoppingListService = ShoppingListService(_authService);
    _loadShoppingLists();
  }

  Future<void> _loadShoppingLists() async {
    setState(() => _isLoading = true);
    final lists = await _shoppingListService.getUserShoppingLists();
    setState(() {
      _shoppingLists = lists;
      _isLoading = false;
    });
  }

  Future<void> _markAsBought(int itemId) async {
    final success = await _shoppingListService.markItemAsBought(itemId);
    if (success) {
      _loadShoppingLists();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Danh sách mua sắm'),
        backgroundColor: Colors.green,
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _shoppingLists.isEmpty
              ? const Center(
                  child: Text('Chưa có danh sách mua sắm nào'),
                )
              : RefreshIndicator(
                  onRefresh: _loadShoppingLists,
                  child: ListView.builder(
                    itemCount: _shoppingLists.length,
                    itemBuilder: (context, index) {
                      final list = _shoppingLists[index];
                      return ShoppingListCard(
                        shoppingList: list,
                        onMarkBought: _markAsBought,
                      );
                    },
                  ),
                ),
    );
  }
}

class ShoppingListCard extends StatelessWidget {
  final dynamic shoppingList;
  final Function(int) onMarkBought;

  const ShoppingListCard({
    super.key,
    required this.shoppingList,
    required this.onMarkBought,
  });

  @override
  Widget build(BuildContext context) {
    final items = shoppingList['shoppingListItems'] as List<dynamic>? ?? [];
    final boughtCount = items.where((item) => item['isBought'] == true).length;

    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      child: ExpansionTile(
        title: Text(
          'Danh sách #${shoppingList['shoppingListId']}',
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
        subtitle: Text('$boughtCount/${items.length} đã mua'),
        children: [
          Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: items.isEmpty
                  ? [const Text('Danh sách trống')]
                  : items.map<Widget>((item) {
                      return ShoppingListItemTile(
                        item: item,
                        onMarkBought: () =>
                            onMarkBought(item['shoppingListItemId']),
                      );
                    }).toList(),
            ),
          ),
        ],
      ),
    );
  }
}

class ShoppingListItemTile extends StatelessWidget {
  final dynamic item;
  final VoidCallback onMarkBought;

  const ShoppingListItemTile({
    super.key,
    required this.item,
    required this.onMarkBought,
  });

  @override
  Widget build(BuildContext context) {
    final isBought = item['isBought'] == true;

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Row(
        children: [
          Checkbox(
            value: isBought,
            onChanged: (_) => onMarkBought(),
          ),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  item['ingredientName'] ?? 'Nguyên liệu',
                  style: TextStyle(
                    decoration: isBought ? TextDecoration.lineThrough : null,
                    color: isBought ? Colors.grey : Colors.black,
                  ),
                ),
                Text(
                  '${item['quantity']} ${item['unitName']}',
                  style: const TextStyle(
                    fontSize: 12,
                    color: Colors.grey,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
