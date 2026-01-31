import '../datasources/remote_datasource.dart';
import '../models/ingredient_model.dart';

class IngredientRepository {
  final RemoteDataSource remote;

  IngredientRepository(this.remote);

  Future<List<IngredientModel>> getAll() async {
    final raw = await remote.fetchIngredients();
    return raw
        .map((e) => IngredientModel.fromJson(Map<String, dynamic>.from(e)))
        .toList();
  }
}
