import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../../core/services/auth_service.dart';
import '../../core/services/meal_plan_service.dart';

class MealPlanPage extends StatefulWidget {
  const MealPlanPage({super.key});

  @override
  State<MealPlanPage> createState() => _MealPlanPageState();
}

class _MealPlanPageState extends State<MealPlanPage> {
  late AuthService _authService;
  late MealPlanService _mealPlanService;
  List<dynamic> _mealPlans = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _authService = AuthService();
    _init();
  }

  Future<void> _init() async {
    await _authService.init();
    _mealPlanService = MealPlanService(_authService);
    _loadMealPlans();
  }

  Future<void> _loadMealPlans() async {
    setState(() => _isLoading = true);
    final mealPlans = await _mealPlanService.getUserMealPlans();
    setState(() {
      _mealPlans = mealPlans;
      _isLoading = false;
    });
  }

  Future<void> _deleteMealPlan(int mealPlanId) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Xóa kế hoạch'),
        content: const Text('Bạn có chắc chắn muốn xóa kế hoạch này?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Hủy'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Xóa', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );

    if (confirmed == true) {
      final success = await _mealPlanService.deleteMealPlan(mealPlanId);
      if (success) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Đã xóa kế hoạch')),
          );
          _loadMealPlans();
        }
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Kế hoạch bữa ăn'),
        backgroundColor: Colors.green,
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _mealPlans.isEmpty
              ? const Center(
                  child: Text('Chưa có kế hoạch bữa ăn nào'),
                )
              : RefreshIndicator(
                  onRefresh: _loadMealPlans,
                  child: ListView.builder(
                    itemCount: _mealPlans.length,
                    itemBuilder: (context, index) {
                      final plan = _mealPlans[index];
                      return MealPlanCard(
                        mealPlan: plan,
                        onDelete: () => _deleteMealPlan(plan['mealPlanId']),
                      );
                    },
                  ),
                ),
    );
  }
}

class MealPlanCard extends StatelessWidget {
  final dynamic mealPlan;
  final VoidCallback onDelete;

  const MealPlanCard({
    super.key,
    required this.mealPlan,
    required this.onDelete,
  });

  @override
  Widget build(BuildContext context) {
    final date = DateTime.parse(mealPlan['plannedDate']);
    final formattedDate = DateFormat('dd/MM/yyyy').format(date);

    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        mealPlan['recipeName'] ?? 'Công thức',
                        style: const TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      const SizedBox(height: 4),
                      Text(
                        'Ngày: $formattedDate',
                        style: const TextStyle(
                          fontSize: 12,
                          color: Colors.grey,
                        ),
                      ),
                    ],
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.delete, color: Colors.red),
                  onPressed: onDelete,
                ),
              ],
            ),
            const SizedBox(height: 12),
            Row(
              children: [
                const Icon(Icons.people, size: 16, color: Colors.green),
                const SizedBox(width: 4),
                Text('${mealPlan['servings']} khẩu phần'),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
