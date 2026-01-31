import 'package:dio/dio.dart';
import 'auth_service.dart';
import '../constants/constants.dart';

class MealPlanService {
  final AuthService authService;
  final Dio _dio = Dio();

  MealPlanService(this.authService);

  Future<List<dynamic>> getUserMealPlans() async {
    try {
      final response = await _dio.get(
        '${ApiConstants.baseUrl}mealplan',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      if (response.statusCode == 200) {
        return response.data ?? [];
      }
      return [];
    } catch (e) {
      print('Get meal plans error: $e');
      return [];
    }
  }

  Future<bool> addMealPlan(
      int recipeId, DateTime plannedDate, int servings) async {
    try {
      final response = await _dio.post(
        '${ApiConstants.baseUrl}mealplan',
        data: {
          'recipeId': recipeId,
          'plannedDate': plannedDate.toIso8601String(),
          'servings': servings,
        },
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      return response.statusCode == 201 || response.statusCode == 200;
    } catch (e) {
      print('Add meal plan error: $e');
      return false;
    }
  }

  Future<bool> deleteMealPlan(int mealPlanId) async {
    try {
      final response = await _dio.delete(
        '${ApiConstants.baseUrl}mealplan/$mealPlanId',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      return response.statusCode == 204 || response.statusCode == 200;
    } catch (e) {
      print('Delete meal plan error: $e');
      return false;
    }
  }
}
