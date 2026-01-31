import 'package:dio/dio.dart';
import 'auth_service.dart';
import '../constants/constants.dart';

class ShoppingListService {
  final AuthService authService;
  final Dio _dio = Dio();

  ShoppingListService(this.authService);

  Future<List<dynamic>> getUserShoppingLists() async {
    try {
      final response = await _dio.get(
        '${ApiConstants.baseUrl}shoppinglist',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      if (response.statusCode == 200) {
        return response.data ?? [];
      }
      return [];
    } catch (e) {
      print('Get shopping lists error: $e');
      return [];
    }
  }

  Future<bool> generateFromMealPlan(int mealPlanId) async {
    try {
      final response = await _dio.post(
        '${ApiConstants.baseUrl}shoppinglist/generate/$mealPlanId',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      return response.statusCode == 201 || response.statusCode == 200;
    } catch (e) {
      print('Generate shopping list error: $e');
      return false;
    }
  }

  Future<bool> markItemAsBought(int itemId) async {
    try {
      final response = await _dio.patch(
        '${ApiConstants.baseUrl}shoppinglist/item/$itemId',
        data: {'isBought': true},
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      return response.statusCode == 200;
    } catch (e) {
      print('Mark item as bought error: $e');
      return false;
    }
  }
}
