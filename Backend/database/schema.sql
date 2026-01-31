-- SQL schema for BepTroLy
-- Users
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    FullName NVARCHAR(100),
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

CREATE TABLE UserSettings (
    SettingId INT IDENTITY PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(UserId),
    NotifyBeforeDays INT DEFAULT 2,
    AllowNotification BIT DEFAULT 1,
    Language NVARCHAR(20) DEFAULT 'vi'
);

-- Ingredients
CREATE TABLE IngredientCategories (
    CategoryId INT IDENTITY PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL
);

CREATE TABLE Units (
    UnitId INT IDENTITY PRIMARY KEY,
    UnitName NVARCHAR(20) NOT NULL
);

CREATE TABLE Ingredients (
    IngredientId INT IDENTITY PRIMARY KEY,
    IngredientName NVARCHAR(100) NOT NULL,
    CategoryId INT NULL REFERENCES IngredientCategories(CategoryId)
);

CREATE TABLE UserIngredients (
    UserIngredientId INT IDENTITY PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(UserId),
    IngredientId INT NOT NULL REFERENCES Ingredients(IngredientId),
    Quantity FLOAT DEFAULT 0,
    UnitId INT NULL REFERENCES Units(UnitId),
    PurchaseDate DATE NULL,
    ExpiryDate DATE NULL,
    IsDeleted BIT DEFAULT 0
);

-- Recipes
CREATE TABLE RecipeCategories (
    CategoryId INT IDENTITY PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL
);

CREATE TABLE Recipes (
    RecipeId INT IDENTITY PRIMARY KEY,
    RecipeName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CookingTime INT NULL,
    CategoryId INT NULL REFERENCES RecipeCategories(CategoryId),
    ImageUrl NVARCHAR(255) NULL,
    VideoUrl NVARCHAR(255) NULL
);

CREATE TABLE RecipeIngredients (
    RecipeIngredientId INT IDENTITY PRIMARY KEY,
    RecipeId INT NOT NULL REFERENCES Recipes(RecipeId),
    IngredientId INT NOT NULL REFERENCES Ingredients(IngredientId),
    RequiredQuantity FLOAT DEFAULT 0,
    UnitId INT NULL REFERENCES Units(UnitId)
);

CREATE TABLE RecipeSteps (
    StepId INT IDENTITY PRIMARY KEY,
    RecipeId INT NOT NULL REFERENCES Recipes(RecipeId),
    StepNumber INT NOT NULL,
    Description NVARCHAR(MAX) NULL
);

-- Recommendations / AI
CREATE TABLE RecipeRecommendations (
    RecommendationId INT IDENTITY PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(UserId),
    RecipeId INT NOT NULL REFERENCES Recipes(RecipeId),
    MatchPercent FLOAT DEFAULT 0,
    MissingIngredients NVARCHAR(255) NULL,
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

-- Expiry notifications
CREATE TABLE ExpiryNotifications (
    NotificationId INT IDENTITY PRIMARY KEY,
    UserIngredientId INT NOT NULL REFERENCES UserIngredients(UserIngredientId),
    NotifyDate DATE NOT NULL,
    IsSent BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

-- Meal plans & shopping
CREATE TABLE MealPlans (
    MealPlanId INT IDENTITY PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(UserId),
    RecipeId INT NOT NULL REFERENCES Recipes(RecipeId),
    PlannedDate DATE NULL,
    MealType NVARCHAR(20) NULL
);

CREATE TABLE ShoppingLists (
    ShoppingListId INT IDENTITY PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(UserId),
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

CREATE TABLE ShoppingListItems (
    ItemId INT IDENTITY PRIMARY KEY,
    ShoppingListId INT NOT NULL REFERENCES ShoppingLists(ShoppingListId),
    IngredientName NVARCHAR(100) NOT NULL,
    Quantity FLOAT DEFAULT 0,
    Unit NVARCHAR(20) NULL,
    IsChecked BIT DEFAULT 0
);

-- Example trigger: auto mark UserIngredient IsDeleted when Quantity <= 0
-- Note: adjust logic as needed
CREATE TRIGGER TR_UserIngredient_Quantity_Update
ON UserIngredients
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ui
    SET IsDeleted = 1
    FROM UserIngredients ui
    INNER JOIN inserted i ON ui.UserIngredientId = i.UserIngredientId
    WHERE i.Quantity <= 0;
END;
