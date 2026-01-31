# BepTroLy Frontend - Flutter App

Giao diện Flutter cho ứng dụng quản lý công thức nấu ăn.

## Tính năng

- **Xác thực**: Đăng nhập và đăng ký người dùng
- **Danh sách công thức**: Xem và tìm kiếm các công thức nấu ăn
- **Chi tiết công thức**: Xem nguyên liệu, các bước nấu, calo
- **Kế hoạch bữa ăn**: Lên kế hoạch bữa ăn hàng ngày
- **Danh sách mua sắm**: Tạo danh sách mua sắm từ kế hoạch bữa ăn

## Cấu trúc Dự án

```
lib/
├── main.dart
├── app.dart
├── core/
│   ├── constants/
│   │   └── constants.dart        # API endpoints, strings
│   ├── services/
│   │   ├── auth_service.dart    # Xác thực người dùng
│   │   ├── recipe_service.dart  # Quản lý công thức
│   │   ├── meal_plan_service.dart  # Quản lý kế hoạch bữa ăn
│   │   └── shopping_list_service.dart  # Quản lý danh sách mua sắm
│   ├── theme/
│   │   └── app_theme.dart       # Thiết kế ứng dụng
│   └── utils/
├── presentation/
│   ├── auth/
│   │   ├── auth_page.dart       # Màn hình đăng nhập
│   │   └── register_page.dart   # Màn hình đăng ký
│   └── pantry/
│       ├── home_page.dart       # Trang chủ
│       ├── recipe_detail_page.dart  # Chi tiết công thức
│       ├── meal_plan_page.dart  # Kế hoạch bữa ăn
│       └── shopping_list_page.dart  # Danh sách mua sắm
├── data/
└── routes/
    └── app_routes.dart          # Định tuyến ứng dụng
```

## Yêu cầu

- Flutter 3.0+
- Dart 2.18+

## Cài đặt

1. Clone repository:
```bash
git clone <repo-url>
cd BepTroLy/Frontend
```

2. Cài đặt dependencies:
```bash
flutter pub get
```

3. Cập nhật backend URL trong `lib/core/constants/constants.dart`:
```dart
static const String baseUrl = 'http://localhost:5000/api/';
```

## Chạy Ứng dụng

```bash
# Chạy trong chế độ development
flutter run

# Chạy trên thiết bị cụ thể
flutter run -d <device-id>

# Chạy production build
flutter build apk
flutter build ios
```

## API Endpoints

Ứng dụng sử dụng các endpoint sau từ backend ASP.NET Core:

- `POST /auth/login` - Đăng nhập
- `POST /auth/register` - Đăng ký
- `GET /recipe` - Lấy danh sách công thức
- `GET /recipe/{id}` - Lấy chi tiết công thức
- `POST /recipe/search` - Tìm kiếm công thức
- `GET /mealplan` - Lấy kế hoạch bữa ăn
- `POST /mealplan` - Thêm kế hoạch bữa ăn
- `DELETE /mealplan/{id}` - Xóa kế hoạch bữa ăn
- `GET /shoppinglist` - Lấy danh sách mua sắm
- `POST /shoppinglist/generate/{mealPlanId}` - Tạo danh sách mua sắm từ kế hoạch
- `PATCH /shoppinglist/item/{id}` - Đánh dấu mục đã mua

## Ghi chú

- Token JWT được lưu trữ trong `SharedPreferences`
- Tất cả các yêu cầu API được xác thực bằng JWT Bearer token
- UI tuân theo Material Design 3

## Hỗ trợ

Nếu gặp vấn đề, vui lòng kiểm tra:
- Backend đang chạy ở `http://localhost:5000`
- URL backend đúng trong `constants.dart`
- Flutter SDK được cài đặt đúng
