# BepTroLy - Ứng Dụng Quản Lý Meal Plan

Ứng dụng di động toàn diện hỗ trợ việc lập kế hoạch bữa ăn, quản lý tài nguyên, và tối ưu hóa danh sách mua sắm với công nghệ AI.

## 📋 Mục Đích Dự Án

BepTroLy là một nền tảng thông minh giúp người dùng:
- 🍽️ Lập kế hoạch bữa ăn hàng tuần một cách tối ưu
- 📖 Quản lý công thức nấu ăn và nguyên liệu sẵn có
- 🛒 Tự động tạo danh sách mua sắm
- 🤖 Nhận khuyến nghị cá nhân hóa dựa trên sở thích và dinh dưỡng
- ⏰ Theo dõi hạn sử dụng của nguyên liệu và nhận thông báo kịp thời

## 🏗️ Kiến Trúc Dự Án

Dự án sử dụng kiến trúc **Clean Architecture** với cách tách biệt rõ ràng giữa các tầng:

```
BepTroLy/
├── Backend/                      # Nền tảng .NET
│   ├── BepTroLy.API/            # Web API Controllers & Startup
│   ├── BepTroLy.Application/    # Business Logic & DTOs
│   ├── BepTroLy.Domain/         # Core Entities & Interfaces
│   └── BepTroLy.Infrastructure/ # Data Access & External Services
│
└── Frontend/                      # Ứng dụng Flutter
    ├── lib/
    │   ├── main.dart            # Điểm vào chính
    │   ├── app.dart             # Cấu hình ứng dụng
    │   ├── core/                # Tiện ích & hằng số chung
    │   ├── data/                # Data Source & Repository
    │   ├── presentation/        # UI & State Management
    │   └── routes/              # Navigation & Routing
    └── pubspec.yaml             # Package Dependencies
```

## 🚀 Bắt Đầu Nhanh

### 📦 Yêu Cầu Hệ Thống

**Backend:**
- .NET 6.0 hoặc cao hơn
- SQL Server hoặc LocalDB
- Visual Studio 2022 hoặc Visual Studio Code

**Frontend:**
- Flutter SDK 3.0 trở lên
- Android SDK (cho Android) hoặc Xcode (cho iOS)
- iOS 11+ hoặc Android 6.0+

### 🔧 Cài Đặt & Chạy Backend

1. **Clone và vào thư mục Backend:**
   ```bash
   cd Backend
   ```

2. **Khôi phục Dependencies:**
   ```bash
   dotnet restore BepTroLy.sln
   ```

3. **Cấu hình Database:**
   - Chỉnh sửa connection string trong `BepTroLy.API/appsettings.json`
   - Hoặc sử dụng LocalDB mặc định được cấu hình trong `Program.cs`

4. **Chạy Database Migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Khởi động API Server:**
   ```bash
   dotnet run --project BepTroLy.API
   ```
   > Server sẽ chạy tại `https://localhost:5001`

### 📱 Cài Đặt & Chạy Frontend

1. **Cài đặt Dependencies:**
   ```bash
   cd Frontend
   flutter pub get
   ```

2. **Chạy ứng dụng:**
   ```bash
   flutter run
   ```
   > Chọn thiết bị hoặc bộ mô phỏng khi được yêu cầu

3. **Build Release (tùy chọn):**
   ```bash
   # Android APK
   flutter build apk --release

   # iOS IPA
   flutter build ios --release
   ```

## 📁 Cấu Trúc Chi Tiết

### Backend Components

| Thư Mục | Mô Tả |
|---------|-------|
| **Controllers/** | API endpoints xử lý HTTP requests |
| **Services/** | Business logic & data processing |
| **DTOs/** | Data Transfer Objects cho API responses |
| **Hubs/** | SignalR hubs cho real-time notifications |
| **Middlewares/** | Exception handling & request processing |
| **BackgroundServices/** | Scheduled tasks (e.g., expiry checks) |
| **Repositories/** | Data access layer |
| **Entities/** | Domain models & database entities |

### Frontend Components

| Thư Mục | Mô Tả |
|---------|-------|
| **presentation/** | UI screens & widgets, state management |
| **data/** | API clients, local storage, repositories |
| **core/** | Utilities, constants, theme, helpers |
| **routes/** | Navigation & routing configuration |
| **models/** | Data models & entities |

## ✨ Các Tính Năng Chính

| Tính Năng | Mô Tả |
|-----------|-------|
| 🔐 **Xác Thực & Phân Quyền** | Đăng nhập an toàn, JWT tokens |
| 📖 **Quản Lý Công Thức** | CRUD recipe, upload hình ảnh |
| 🍽️ **Lập Kế Hoạch Bữa Ăn** | Lên lịch bữa ăn hàng tuần |
| 🛒 **Danh Sách Mua Sắm** | Tự động tạo từ meal plan |
| 🥫 **Theo Dõi Nguyên Liệu** | Quản lý tồn kho & hạn sử dụng |
| 🤖 **Khuyến Nghị Thông Minh** | AI recommendations dựa trên sở thích |
| 🔔 **Thông Báo Real-time** | SignalR notifications |
| 📊 **Thống Kê & Insights** | Phân tích dinh dưỡng & chi phí |

## 🛠️ Stack Công Nghệ

### Backend
```
ASP.NET Core 6+     → Web Framework
Entity Framework Core → ORM & Database
SignalR            → Real-time Communication
SQL Server / LocalDB → Database
Swagger / OpenAPI  → API Documentation
```

### Frontend
```
Flutter 3.0+       → Cross-platform UI Framework
Dart               → Programming Language
GetX / Riverpod    → State Management
Dio                → HTTP Client
SQLite             → Local Storage
```

## 📐 Quy Ước Phát Triển

### Git Workflow
- **Feature branches:** `feature/feature-name`
- **Bug fix branches:** `bugfix/bug-name`
- **Commit messages:** Sử dụng Conventional Commits
  - `feat: thêm tính năng mới`
  - `fix: sửa lỗi`
  - `docs: cập nhật tài liệu`

### Code Style
- **Backend:** Theo hướng dẫn C# của Microsoft
- **Frontend:** Theo Effective Dart style guide
- **Naming:** camelCase cho biến/hàm, PascalCase cho classes

### Testing
- Unit tests cho business logic
- Widget tests cho UI components
- Integration tests cho critical flows

## 📚 API Documentation

API documentation được tạo tự động bằng Swagger/OpenAPI. Truy cập tại:
```
https://localhost:5001/swagger
```

## 🐛 Troubleshooting

### Backend Issues
| Vấn Đề | Giải Pháp |
|--------|----------|
| Connection string không hợp lệ | Kiểm tra appsettings.json |
| Database migration failed | Xóa database cũ và chạy lại migration |
| Port 5001 đã bị sử dụng | Thay đổi port trong launchSettings.json |

### Frontend Issues
| Vấn Đề | Giải Pháp |
|--------|----------|
| Dependencies conflict | Chạy `flutter pub get` |
| Build failed | Chạy `flutter clean` rồi `flutter pub get` |
| Emulator issues | Kiểm tra device: `flutter devices` |

## 🤝 Đóng Góp

Chúng tôi hoan nghênh mọi đóng góp! Vui lòng:

1. **Fork** repository
2. Tạo **feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit** thay đổi (`git commit -m 'feat: add some AmazingFeature'`)
4. **Push** đến branch (`git push origin feature/AmazingFeature`)
5. Mở **Pull Request**

### Pull Request Guidelines
- Mô tả rõ ràng mục đích của PR
- Liên kết các related issues
- Thêm screenshots/videos nếu là UI changes
- Đảm bảo tất cả tests pass

## 📋 Roadmap

- [ ] V1.0 - Core features (Recipe, Meal Plan, Shopping List)
- [ ] V1.1 - AI recommendations
- [ ] V1.2 - Social features (sharing recipes, meal plans)
- [ ] V2.0 - Web version
- [ ] V2.1 - Advanced analytics & nutrition tracking

## Thành viên
- Dương Văn Việt
- Phạm Đức Duy Tiến
- Vương Đức Tuấn
---

<div align="center">

**[⬆ Lên đầu](#beptroly---ứng-dụng-quản-lý-meal-plan)**

Yêu thích dự án? Hãy ⭐ nó!

</div>

