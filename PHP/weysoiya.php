<?php
  $input=$_POST['wey'];
  $yo=$_POST['yo'];
  $ja=$_POST['ja'];

  function utf8_ord($char) {
    if (1 !== mb_strlen($char, 'UTF-8')) {
      return ''; 
    }
    $ret = mb_convert_encoding($char, 'UTF-32BE', 'UTF-8');
    return bin2hex($ret); 
  }

  function utf8_chr($cp) {
    if ($cp < 0 || 0x10FFFF < $cp || (0xD800 <= $cp && $cp <= 0xDFFF)) {
      return '';
    }
    $ret = str_repeat('0', 8 - strlen(dechex($cp))).dechex($cp);
    $ret = hex2bin($ret);
    $ret = mb_convert_encoding($ret, 'UTF-8', 'UTF-32BE');
    return $ret; 
  } 

  function encode($text){
    for($i=0;$i<mb_strlen($text,'UTF-8');$i++){
      $hex=utf8_ord(mb_substr($text,$i,1,'UTF-8'));
      $hex=mb_substr($hex,2,6,'UTF-8');
      for($j=0;$j<6;$j++){
        switch(hexdec(mb_substr($hex,$j,1,'UTF-8'))){
          case 0: echo('ウェイ'); break;
          case 1: echo('ウェい'); break;
          case 2: echo('ウぇイ'); break;
          case 3: echo('ウぇい'); break;
          case 4: echo('うェイ'); break;
          case 5: echo('うェい'); break;
          case 6: echo('うぇイ'); break;
          case 7: echo('うぇい'); break;
          case 8: echo('ソイヤ'); break;
          case 9: echo('ソイや'); break;
          case 10: echo('ソいヤ'); break;
          case 11: echo('ソいや'); break;
          case 12: echo('そイヤ'); break;
          case 13: echo('そイや'); break;
          case 14: echo('そいヤ'); break;
          case 15: echo('そいや'); break;
        }
      }
    }
  }

  function decode($text){
    $hex="";
    for($i=0;$i<mb_strlen($text,'UTF-8')/3;$i++){
      switch(mb_substr($text,$i*3,3,'UTF-8')){
        case 'ウェイ': $hex.='0'; break;
        case 'ウェい': $hex.='1'; break;
        case 'ウぇイ': $hex.='2'; break;
        case 'ウぇい': $hex.='3'; break;
        case 'うェイ': $hex.='4'; break;
        case 'うェい': $hex.='5'; break;
        case 'うぇイ': $hex.='6'; break;
        case 'うぇい': $hex.='7'; break;
        case 'ソイヤ': $hex.='8'; break;
        case 'ソイや': $hex.='9'; break;
        case 'ソいヤ': $hex.='a'; break;
        case 'ソいや': $hex.='b'; break;
        case 'そイヤ': $hex.='c'; break;
        case 'そイや': $hex.='d'; break;
        case 'そいヤ': $hex.='e'; break;
        case 'そいや': $hex.='f'; break;
      }
      if(mb_strlen($hex)>5){
        echo(utf8_chr('00'.hexdec($hex)));
        $hex="";
      }
    }
  }

?>

<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
  <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no">
  <title>ウェイソイヤ</title>
</head>

<body>
  this is wey-soiya converter in php
by post sending<br><br>
  <form action="./weysoiya.php" method="post">
input
    <textarea name="wey" cols="40" rows="8">
<?php
  if($input){
    echo($input);
  }
?>
</textarea><br><br><br>
    <input type="submit" value="日本語化" name="ja">
    <input type="submit" value="ウェイ化" name="yo"><br><br><br>
output
    <textarea cols="40" rows="8">
<?php
  if($input){
    if($yo){
      encode($input);
    }elseif($ja){
      decode($input);
    }
  }
?>
</textarea>
  </form>
</body>
</html>