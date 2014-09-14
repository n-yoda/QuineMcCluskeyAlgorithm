QuineMcCluskeyAlgorithm
=======================

QM法( http://ja.wikipedia.org/wiki/%E3%82%AF%E3%83%AF%E3%82%A4%E3%83%B3%E3%83%BB%E3%83%9E%E3%82%AF%E3%83%A9%E3%82%B9%E3%82%AD%E3%83%BC%E6%B3%95 )C#で実装したものです。
不要にLINQを多用しているので遅いです。

## 実行オプション
  -n [Truth Table]などのキャプションを出力しない

  QM法適用前
  -T 真理値表
  -D 主加法標準形
  -C 主乗法標準形

  QM法適用後
  -qT QM法的用後の、真理値表
  -qD QM法的用後の、主加法標準形

  チェック用
  -cT -qDの結果から作った、真理値表
  -cD -qDの結果から作った、主加法標準形
  -cC -qDの結果から作った、主乗法標準形

## 実行例（ADDERの下から3ビット目）
  xbuild QuineMcCluskeyAlgorithm.sln
  cd QuineMcCluskeyAlgorithm/bin/Debug/
  mono QuineMcCluskeyAlgorithm.exe -T -qT
  (a2^b2)^((a1*b1)+(a1*(a0*b0))+(b1*(a0*b0)))

  [Truth Table]
   a0 a1 a2 b0 b1 b2
    0  0  0  0  0  0  0
    1  0  0  0  0  0  0
    0  1  0  0  0  0  0
    1  1  0  0  0  0  0
    0  0  1  0  0  0  1
    1  0  1  0  0  0  1
    0  1  1  0  0  0  1
    1  1  1  0  0  0  1
    0  0  0  1  0  0  0
    1  0  0  1  0  0  0
    0  1  0  1  0  0  0
    1  1  0  1  0  0  1
    0  0  1  1  0  0  1
    1  0  1  1  0  0  1
    0  1  1  1  0  0  1
    1  1  1  1  0  0  0
    0  0  0  0  1  0  0
    1  0  0  0  1  0  0
    0  1  0  0  1  0  1
    1  1  0  0  1  0  1
    0  0  1  0  1  0  1
    1  0  1  0  1  0  1
    0  1  1  0  1  0  0
    1  1  1  0  1  0  0
    0  0  0  1  1  0  0
    1  0  0  1  1  0  1
    0  1  0  1  1  0  1
    1  1  0  1  1  0  1
    0  0  1  1  1  0  1
    1  0  1  1  1  0  0
    0  1  1  1  1  0  0
    1  1  1  1  1  0  0
    0  0  0  0  0  1  1
    1  0  0  0  0  1  1
    0  1  0  0  0  1  1
    1  1  0  0  0  1  1
    0  0  1  0  0  1  0
    1  0  1  0  0  1  0
    0  1  1  0  0  1  0
    1  1  1  0  0  1  0
    0  0  0  1  0  1  1
    1  0  0  1  0  1  1
    0  1  0  1  0  1  1
    1  1  0  1  0  1  0
    0  0  1  1  0  1  0
    1  0  1  1  0  1  0
    0  1  1  1  0  1  0
    1  1  1  1  0  1  1
    0  0  0  0  1  1  1
    1  0  0  0  1  1  1
    0  1  0  0  1  1  0
    1  1  0  0  1  1  0
    0  0  1  0  1  1  0
    1  0  1  0  1  1  0
    0  1  1  0  1  1  1
    1  1  1  0  1  1  1
    0  0  0  1  1  1  1
    1  0  0  1  1  1  0
    0  1  0  1  1  1  0
    1  1  0  1  1  1  0
    0  0  1  1  1  1  0
    1  0  1  1  1  1  1
    0  1  1  1  1  1  1
    1  1  1  1  1  1  1
  
  [QM'ed Truth Table]
   a0 a1 a2 b0 b1 b2
    -  -  1  0  0  0  1
    1  1  0  1  -  0  1
    -  0  1  -  0  0  1
    0  -  1  -  0  0  1
    -  1  0  -  1  0  1
    -  0  1  0  -  0  1
    1  -  0  1  1  0  1
    0  0  1  -  -  0  1
    -  -  0  0  0  1  1
    -  0  0  -  0  1  1
    0  -  0  -  0  1  1
    1  1  1  1  -  1  1
    -  0  0  0  -  1  1
    -  1  1  -  1  1  1
    0  0  0  -  -  1  1
    1  -  1  1  1  1  1