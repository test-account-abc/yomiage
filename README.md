# アプリのビルド＆実行手順

## 1. VisualStudio をインストール

- [ここ](https://visualstudio.microsoft.com/ja/downloads/)からインストール
- `Visual Studio Community 2022` > `変更`から以下のコンポーネントをインストール
  - `ワークロード`
    - WinUI アプリケーション開発
  - `インストールの詳細` > `WinUI アプリケーション開発` > `オプション`
    - C++ WinUI アプリ開発ツール
    - ユニバーサル Windows プラットフォーム ツール
    - C++(v143) ユニバーサル Windows プラットフォーム ツール
    - C++(v142) ユニバーサル Windows プラットフォーム ツール
    - C++(v141) ユニバーサル Windows プラットフォーム ツール
    - Windows 11 SDK (10.0.26100.0)
    - Windows 11 SDK (10.0.22621.0)
    - Windows 11 SDK (10.0.22000.0)

## 2. VoicevoxCoreSharp をインストール

- VoicevoxCoreSharp を clone し、特定のコミットでビルドする

```bash
git clone https://github.com/yamachu/VoicevoxCoreSharp.git
cd VoicevoxCoreSharp/src/VoicevoxCoreSharp.Core/
git checkout c26ef70aedfec96fdefa384f5ff27548d6e1c47a
dotnet build
```

- 上記手順で生成された`VoicevoxCoreSharp/src/VoicevoxCoreSharp.Core/bin/Debug/netstandard2.0/VoicevoxCreSharp.Core.dll`をコピー
  - 貼り付け先は`Yomiage/Yomiage/Libs/VoicevoxCoreSharp.Core.dll`

## 3. VoicevoxCore をインストール

- `0.15.0-preview.15`の[リリースタグ](https://github.com/VOICEVOX/voicevox_core/releases/tag/0.15.0-preview.15)から `download-windows-x64.exe` を取得する
- downloader を`Yomiage/Yomiage/download-windows-x64.exe`へ配置する
- 以下のコマンドを実行
  - ```bash
    ./download-windows-x64.exe --version 0.15.0-preview.15
    ```

## 4. アプリのビルド＆実行

- VisualStudio でプロジェクト(Yomiage.sln)を開く
- `ビルド` > `パッケージ Yomiage`をクリック
- `bin/Debug/net8.0-windows10.0.19041.0/win-x64/Yomiage.exe`を実行
