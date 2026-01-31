import 'package:dio/dio.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/constants.dart';

class AuthService {
  final Dio _dio = Dio();
  late SharedPreferences _prefs;

  String? _token;
  String? _userId;
  String? _userName;

  String? get token => _token;
  String? get userId => _userId;
  String? get userName => _userName;
  bool get isAuthenticated => _token != null;

  Future<void> init() async {
    _prefs = await SharedPreferences.getInstance();
    _token = _prefs.getString('auth_token');
    _userId = _prefs.getString('user_id');
    _userName = _prefs.getString('user_name');
  }

  Future<bool> register(String email, String password, String name) async {
    try {
      final response = await _dio.post(
        '${ApiConstants.baseUrl}auth/register',
        data: {
          'email': email,
          'password': password,
          'name': name,
        },
      );

      if (response.statusCode == 200) {
        final data = response.data;
        _token = data['token'];
        _userId = data['user']['userId'].toString();
        _userName = data['user']['name'];

        await _saveToken();
        return true;
      }
      return false;
    } catch (e) {
      print('Register error: $e');
      return false;
    }
  }

  Future<bool> login(String email, String password) async {
    try {
      final response = await _dio.post(
        '${ApiConstants.baseUrl}auth/login',
        data: {
          'email': email,
          'password': password,
        },
      );

      if (response.statusCode == 200) {
        final data = response.data;
        _token = data['token'];
        _userId = data['user']['userId'].toString();
        _userName = data['user']['name'];

        await _saveToken();
        return true;
      }
      return false;
    } catch (e) {
      print('Login error: $e');
      return false;
    }
  }

  Future<void> logout() async {
    _token = null;
    _userId = null;
    _userName = null;
    await _prefs.remove('auth_token');
    await _prefs.remove('user_id');
    await _prefs.remove('user_name');
  }

  Future<void> _saveToken() async {
    if (_token != null) await _prefs.setString('auth_token', _token!);
    if (_userId != null) await _prefs.setString('user_id', _userId!);
    if (_userName != null) await _prefs.setString('user_name', _userName!);
  }

  // Helper for API requests with auth header
  Future<Response> getWithAuth(String path) async {
    return _dio.get(
      '${ApiConstants.baseUrl}$path',
      options: Options(headers: {'Authorization': 'Bearer $_token'}),
    );
  }

  Future<Response> postWithAuth(String path, dynamic data) async {
    return _dio.post(
      '${ApiConstants.baseUrl}$path',
      data: data,
      options: Options(headers: {'Authorization': 'Bearer $_token'}),
    );
  }
}
