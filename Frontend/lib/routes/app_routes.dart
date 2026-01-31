import 'package:flutter/material.dart';
import '../presentation/auth/auth_page.dart';
import '../presentation/auth/register_page.dart';
import '../presentation/pantry/home_page.dart';
import '../presentation/pantry/recipe_detail_page.dart';
import '../presentation/pantry/meal_plan_page.dart';
import '../presentation/pantry/shopping_list_page.dart';

class AppRoutes {
  static const login = '/login';
  static const register = '/register';
  static const home = '/home';
  static const recipeDetail = '/recipe-detail';
  static const mealPlan = '/meal-plan';
  static const shoppingList = '/shopping-list';

  static Map<String, WidgetBuilder> getRoutes() {
    return {
      login: (context) => const LoginPage(),
      register: (context) => const RegisterPage(),
      home: (context) => const HomePage(),
      mealPlan: (context) => const MealPlanPage(),
      shoppingList: (context) => const ShoppingListPage(),
      recipeDetail: (context) {
        final recipeId =
            ModalRoute.of(context)?.settings.arguments as int? ?? 0;
        return RecipeDetailPage(recipeId: recipeId);
      },
    };
  }
}
