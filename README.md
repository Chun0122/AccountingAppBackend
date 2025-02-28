# 會計應用程式 (Accounting App)

這是一個簡單的記帳網頁應用程式，旨在幫助使用者輕鬆管理他們的個人財務。

後端位置：[https://github.com/Chun0122/AccountingAppBackend]

## 功能特色

- **新增帳目：** 使用者可以新增收入或支出的詳細記錄，包括日期、類別、金額和描述。
- **檢視帳目：** 使用者可以檢視所有帳目記錄，並按照日期或類別進行排序和篩選。
- **帳目統計：** 提供基本的統計功能，例如總收入、總支出和餘額。
- **圖表視覺化：** 透過直觀的圖表展示收支趨勢和分類統計。
- **使用者友善介面：** 簡潔直觀的使用者介面，方便使用者快速上手。
- **雲端部署：** 應用程式已部署至Azure平台，隨時隨地可以訪問。
- **CI/CD自動化：** 通過GitHub Actions實現持續整合和持續部署，確保代碼品質和自動化部署。

## 線上示範

可以通過以下鏈接訪問應用程式的線上版本：
[https://brave-water-0ffc2560f.6.azurestaticapps.net](https://brave-water-0ffc2560f.6.azurestaticapps.net)

## 技術棧

- **前端：**
  - React 18.2.0
  - React Router 6.10.0
  - Redux Toolkit 1.9.5 (狀態管理)
  - Chart.js 4.2.1 (資料視覺化)
  - Material UI 5.12.1 (UI元件庫)
  - Formik 2.2.9 (表單管理)
  - Yup 1.1.1 (表單驗證)
  - Dayjs 1.11.7 (日期處理)

- **後端：**
  - .NET 8.0 Web API
  - Entity Framework Core 6.0
  - ASP.NET Core Identity
  - AutoMapper
  - FluentValidation
  - Swagger/OpenAPI
  - Serilog (日誌記錄)

- **資料庫：**
  - MySQL 8.0

- **部署環境：**
  - 前端: Azure Static Web Apps
  - 後端: Azure App Service
  - 資料庫: Azure Database for MySQL

- **CI/CD：**
  - GitHub Actions (自動化工作流程)
  - ESLint & Prettier (前端代碼品質檢查)
  - .NET Tests & Code Analysis (後端代碼品質檢查)
  - SonarCloud (代碼質量分析)

## 安裝與執行

### 前端

1.  複製前端專案到本地：

    ```bash
    git clone https://github.com/Chun0122/accounting-app.git
    ```

2.  進入前端專案目錄：

    ```bash
    cd accounting-app
    ```

3.  安裝相依套件：

    ```bash
    npm install
    ```

4.  啟動開發伺服器：

    ```bash
    npm start
    ```

5.  在瀏覽器中開啟 `http://localhost:3000`。

### 後端

1.  複製後端專案到本地：

    ```bash
    git clone https://github.com/Chun0122/AccountingAppBackend.git
    ```

2.  進入後端專案目錄：

    ```bash
    cd AccountingAppBackend
    ```

3.  使用Visual Studio 2022或Visual Studio Code開啟解決方案。

4.  設定資料庫連接字串：
    - 打開 `appsettings.json` 文件
    - 修改 `ConnectionStrings` 區段以設定MySQL連接字串：
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=AccountingApp;Uid=root;Pwd=yourpassword;"
    }
    ```

5.  執行資料庫遷移：
    ```bash
    dotnet ef database update
    ```

6.  運行應用程式：
    ```bash
    dotnet run
    ```

7.  後端API預設運行在 `https://localhost:7000` 和 `http://localhost:5000`。

## 專案結構

- **前端：**
  - `src/`: 包含 React 應用程式的原始碼。
    - `components/`: 包含可重用的 UI 元件。
    - `pages/`: 包含主要頁面元件。
    - `redux/`: 包含 Redux 狀態管理相關檔案。
      - `slices/`: Redux Toolkit 切片。
      - `store.js`: Redux store 配置。
    - `services/`: API 服務和數據處理。
    - `utils/`: 工具函數和輔助方法。
    - `hooks/`: 自定義 React Hooks。
    - `assets/`: 靜態資源如圖片和樣式檔。
    - `constants/`: 常數定義。
    - `App.js`: 主應用元件。
    - `index.js`: 應用入口點。
  - `.github/workflows/`: GitHub Actions CI/CD 工作流程配置。

- **後端：**
  - `AccountingApp.API/`: 包含Web API專案。
    - `Controllers/`: API控制器。
    - `Models/`: 數據模型。
    - `DTOs/`: 數據傳輸對象。
    - `Services/`: 業務邏輯服務。
    - `Repositories/`: 數據存取層。
    - `Middleware/`: 自定義中間件。
    - `Extensions/`: 擴展方法。
    - `Helpers/`: 輔助類和方法。
  - `AccountingApp.Core/`: 包含核心領域模型和接口。
  - `AccountingApp.Infrastructure/`: 包含數據庫上下文和配置。
  - `AccountingApp.Tests/`: 包含單元測試和整合測試。
  - `.github/workflows/`: GitHub Actions CI/CD 工作流程配置。

## CI/CD 流程

本專案採用GitHub Actions實現完整的CI/CD自動化流程：

### 前端CI/CD

```yaml
name: Frontend CI/CD

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]
    paths:
      - 'src/**'
      - 'public/**'
      - 'package.json'
      - '.github/workflows/frontend.yml'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Set up Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
        
    - name: Install dependencies
      run: npm ci
      
    - name: Lint
      run: npm run lint
      
    - name: Test
      run: npm test
      
    - name: Build
      run: npm run build
      
    - name: Deploy to Azure Static Web Apps
      if: github.ref == 'refs/heads/main'
      uses: Azure/static-web-apps-deploy@v1
      with:
        azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
        repo_token: ${{ secrets.GITHUB_TOKEN }}
        action: "upload"
        app_location: "build"
        api_location: ""
        output_location: "build"
```

### 後端CI/CD

```yaml
name: Backend CI/CD

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]
    paths:
      - '**/*.cs'
      - '**/*.csproj'
      - '.github/workflows/backend.yml'

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '6.0.x'
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
  deploy:
    needs: build-and-test
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '6.0.x'
        
    - name: Publish
      run: dotnet publish -c Release -o ./publish
      
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'your-webapi-app-name'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

## 部署說明

### Azure部署步驟

1. **前端部署至Azure Static Web Apps**:
   - 在Azure Portal中創建新的Static Web App資源
   - 連接GitHub/GitLab儲存庫
   - 配置建構設定:
     - 建構指令: `npm run build`
     - 輸出位置: `build`
   - 設定環境變數，特別是API base URL
   - GitHub Actions會自動處理部署流程

2. **後端部署至Azure App Service**:
   - 在Azure Portal中創建新的App Service資源 (Windows或Linux)
   - 配置部署中心連接GitHub/GitLab儲存庫
   - 設定必要的環境變數
   - 啟用CORS設定，允許前端網站訪問
   - GitHub Actions會自動處理部署流程

3. **資料庫設定**:
   - 在Azure Portal中創建Azure Database for MySQL資源
   - 設定防火牆規則，允許App Service訪問
   - 設定連接字串至後端App Service的環境變數

## 系統要求

- 前端開發:
  - Node.js 14.x 或更高版本
  - npm 6.x 或更高版本
- 後端開發:
  - .NET 8.0 SDK
  - Visual Studio 2022 或 Visual Studio Code
- 資料庫:
  - MySQL 8.0 或更高版本
- 支持現代JavaScript的瀏覽器 (Chrome, Firefox, Safari, Edge)
- Git (用於版本控制和CI/CD流程)

## 貢獻

歡迎任何形式的貢獻！如果您發現任何錯誤或有任何改進建議，請隨時提交Pull Request或Issues。

貢獻步驟：
1. Fork此專案
2. 創建您的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交您的更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打開Pull Request

所有Pull Request會自動觸發CI/CD流程進行測試和代碼質量檢查。

## 授權

此專案使用MIT授權。請查看`LICENSE`檔案了解更多資訊。

## 聯絡方式

如有任何問題或建議，請通過以下方式聯繫：
- GitHub Issues: [https://github.com/Chun0122/accounting-app/issues](https://github.com/Chun0122/accounting-app/issues)

## 未來規劃

- 多語言支援
- 深色模式
- 預算設定和提醒功能
- 匯出報表功能
- 行動應用版本
- 改進CI/CD流程，加入自動化E2E測試
- API文檔自動生成和發布

## 注意事項

- 請確保您已安裝正確版本的.NET SDK和Node.js。
- 在執行後端專案之前，請確保已正確設定MySQL資料庫連接資訊。
- 如有任何問題，請隨時提出 Issue。
- 請確保您有相應的Azure部署權限才能使用CI/CD功能。
