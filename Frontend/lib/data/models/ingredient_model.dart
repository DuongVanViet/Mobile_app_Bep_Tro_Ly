class IngredientModel {
  final int id;
  final String name;
  final String unit;

  IngredientModel({required this.id, required this.name, required this.unit});

  factory IngredientModel.fromJson(Map<String, dynamic> json) =>
      IngredientModel(
        id: json['id'] as int,
        name: json['name'] as String,
        unit: json['unit'] as String,
      );

  Map<String, dynamic> toJson() => {'id': id, 'name': name, 'unit': unit};
}
