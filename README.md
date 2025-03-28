# アプリのビルド手順

## 1. VisualStudio をインストール

[ここ](https://visualstudio.microsoft.com/ja/downloads/)からインストール

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
