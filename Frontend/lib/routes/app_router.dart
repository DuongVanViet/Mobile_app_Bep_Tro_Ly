import 'package:flutter/material.dart';
import '../presentation/auth/auth_page.dart';
import '../presentation/pantry/pantry_page.dart';

class AppRouter {
  static Route<dynamic>? onGenerateRoute(RouteSettings settings) {
    switch (settings.name) {
      case '/':
        return MaterialPageRoute(builder: (_) => const PantryPage());
      case '/auth':
        return MaterialPageRoute(builder: (_) => const AuthPage());
      default:
        return MaterialPageRoute(
            builder: (_) =>
                const Scaffold(body: Center(child: Text('Not Found'))));
    }
  }
}
