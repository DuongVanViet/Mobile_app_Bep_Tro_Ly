import '../../core/services/api_service.dart';

class RemoteDataSource {
  final ApiService api;

  RemoteDataSource(this.api);

  Future<List<dynamic>> fetchIngredients() async {
    final res = await api.get('ingredient');
    return List<dynamic>.from(res ?? []);
  }

  Future<List<dynamic>> fetchRecipes() async {
    final res = await api.get('recipe');
    return List<dynamic>.from(res ?? []);
  }
}
