# プロジェクトの概要
Microsoft Power AutomateからNulabが提供するBacklogのデータを扱う場合、Backlog APIを利用してBacklog内のデータにアクセスすることがで来ます。
ただし、一部のAPIはPower Automate側の機能不足によりうまく取り扱えないものが存在します。

このプログラムでは、前述したPower Automate側の問題を回避するためにBacklog APIをブリッジするAPI群を提供するとともに、Power Automateでカスタムコネクタを作成する際に必要となるSwagger仕様書、およびAPIのテストを行うためのSwagger UIを提供します。

# 利用方法
このプログラムは、Power AutomateからBacklogのAPIへのリクエストをブリッジするための機能ですが、すべてのBacklog利用者に公開するための物ではなく、あくまでプロジェクト実行者が関わるBacklogプロジェクトに対してのみ利用する事を想定しています。
そのため、プログラム実行時に許可するBacklogスペースを列挙し、それ以外のBacklogスペースへのリクエストが来た場合はこれを拒否します。

## 設定ファイルで指定する場合

設定ファイルで指定する場合は、プロジェクト直下の`appsettings.json`の`AllowBacklogSpaces`に許可したいBacklogスペースを列挙することで指定できます。

``` appsettngs.json 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowBacklogSpaces": [
      "space1", "space2"
  ],
  "AllowedHosts": "*"
}
```

## 環境変数で指定する場合

環境変数で設定する場合は、下記のように許可したいBacklogスペース毎に`AllowBackogSpaces__連番`を設定することで指定できます。

```
export AllowBacklogSpaces__0=space1
export AllowBacklogSpaces__2=space2
```

