import 'package:dio/dio.dart';
import 'auth_service.dart';
import '../constants/constants.dart';

class RecipeService {
  final AuthService authService;
  final Dio _dio = Dio();

  RecipeService(this.authService);

  Future<List<dynamic>> getAllRecipes() async {
    try {
      final response = await _dio.get(
        '${ApiConstants.baseUrl}recipe',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      if (response.statusCode == 200) {
        return response.data ?? [];
      }
      return [];
    } catch (e) {
      print('Get recipes error: $e');
      return [];
    }
  }

  Future<dynamic> getRecipeById(int recipeId) async {
    try {
      final response = await _dio.get(
        '${ApiConstants.baseUrl}recipe/$recipeId',
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      if (response.statusCode == 200) {
        return response.data;
      }
      return null;
    } catch (e) {
      print('Get recipe detail error: $e');
      return null;
    }
  }

  Future<List<dynamic>> searchRecipes(String query) async {
    try {
      final response = await _dio.post(
        '${ApiConstants.baseUrl}recipe/search',
        data: {'query': query},
        options:
            Options(headers: {'Authorization': 'Bearer ${authService.token}'}),
      );

      if (response.statusCode == 200) {
        return response.data ?? [];
      }
      return [];
    } catch (e) {
      print('Search recipes error: $e');
      return [];
    }
  }
}
