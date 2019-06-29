# CPPl_Remover_Jack
After Effects aepファイルのCPPlタグを初期化します。   
  
気休めですが、少しは読み込みが早くなります。  
  
[【AfterEffects】AEPの起動を遅くする原因、CPPlタグを削除するツールの紹介](https://www.cg-method.com/entry/aftereffects-delete-cppl/)にあったツールを参考に作成しました。  

むらしんさんのCPPl_Remover.exeをC#でUIを作り直しただけなので、機能は同じです。　　
  
僕が1G超えのaepファイルを使ってて、少しでも早くしようと高速化してしてますが、あまり変わってません。  
  
おまけのClearXmpHistory.jsxはaep内のXMPメタデータ内のHistoryをクリアするスクリプトです。  
# Usage
aepをドラッグ＆ドロップして登録するか、selectボタンを押してダイアログで選んでください。
CPPlタグがあればexecボタンで変換です。  
  

一応元ファイルは.backupをつけて保存してあります。  

# License

This software is released under the MIT License, see LICENSE. 

# Authors

bry-ful(Hiroshi Furuhashi) http://bryful.yuzu.bz/  
twitter:[bryful](https://twitter.com/bryful)  
bryful@gmail.com  

# References
 * [aepデータで、読み込みに非常に時間がかかる物がある)](https://forums.adobe.com/thread/2268444)  
 * [【AfterEffects】AEPの起動を遅くする原因、CPPlタグを削除するツールの紹介](https://www.cg-method.com/entry/aftereffects-delete-cppl/)
 * [むらしんさんのツィート](https://twitter.com/murasin/status/1126663479090221056?ref_src=twsrc%5Etfw%7Ctwcamp%5Etweetembed%7Ctwterm%5E1126663479090221056%7Ctwgr%5E393039363b74776565745f6d65646961&ref_url=https%3A%2F%2Fwww.cg-method.com%2Fentry%2Faftereffects-delete-cppl%2F)
 
