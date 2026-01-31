class RecipeModel {
  final int id;
  final String title;
  final List<String> ingredients;

  RecipeModel(
      {required this.id, required this.title, required this.ingredients});

  factory RecipeModel.fromJson(Map<String, dynamic> json) => RecipeModel(
        id: json['id'] as int,
        title: json['title'] as String,
        ingredients: List<String>.from(json['ingredients'] ?? []),
      );

  Map<String, dynamic> toJson() =>
      {'id': id, 'title': title, 'ingredients': ingredients};
}
