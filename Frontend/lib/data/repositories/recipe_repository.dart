import '../datasources/remote_datasource.dart';
import '../models/recipe_model.dart';

class RecipeRepository {
  final RemoteDataSource remote;

  RecipeRepository(this.remote);

  Future<List<RecipeModel>> getAll() async {
    final raw = await remote.fetchRecipes();
    return raw
        .map((e) => RecipeModel.fromJson(Map<String, dynamic>.from(e)))
        .toList();
  }
}
