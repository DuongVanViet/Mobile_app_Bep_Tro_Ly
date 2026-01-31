import 'dart:convert';
import 'package:http/http.dart' as http;
import '../constants/constants.dart';

class ApiService {
  final String baseUrl;

  ApiService({String? base}) : baseUrl = base ?? ApiConstants.baseUrl;

  Future<dynamic> get(String path) async {
    final uri = Uri.parse('$baseUrl$path');
    final res = await http.get(uri);
    if (res.statusCode >= 200 && res.statusCode < 300) {
      return jsonDecode(res.body);
    }
    throw Exception('Request failed: ${res.statusCode}');
  }

  Future<dynamic> post(String path, Map<String, dynamic> body) async {
    final uri = Uri.parse('$baseUrl$path');
    final res = await http.post(uri,
        body: jsonEncode(body), headers: {'Content-Type': 'application/json'});
    if (res.statusCode >= 200 && res.statusCode < 300) {
      return jsonDecode(res.body);
    }
    throw Exception('Request failed: ${res.statusCode}');
  }
}
