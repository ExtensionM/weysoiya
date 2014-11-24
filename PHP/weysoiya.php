<?php
  $q=file(dirname(__FILE__).'/weysoiya4.wss',FILE_IGNORE_NEW_LINES);
  $input=$_POST['wey'];//comment
  $yo=$_POST['yo'];
  $ja=$_POST['ja'];
  $enc=$_POST['enc'];

  echo($q[0]);

  function encode($text,$enc){
    global $q;
    for($i=0;$i<mb_strlen($text,"UTF-8");$i++){
      $hex=bin2hex(mb_convert_encoding(mb_substr($text,$i,1,"UTF-8"),$enc,"UTF-8"));
      for($j=0;$j<8;$j++){
        echo($q[hexdec(mb_substr($hex,$j,1,"UTF-8"))+1]);
      }
    }
  }

  function decode($text,$enc){
    global $q;
    $hex="";
    for($i=0;$i<mb_strlen($text,"UTF-8")/3;$i++){
      $hex.=dechex(array_search(mb_substr($text,$i*3,3,"UTF-8"),$q)-1);
      switch($enc){
        case "UTF-8":
          if(mb_strlen($hex)>5){
            echo(hex2bin($hex));
            $hex="";
          }
          break;
        case "UTF-16LE":
          if(mb_strlen($hex)>3){
            $hex=mb_substr($hex,2,2,"UTF-8").mb_substr($hex,0,2,"UTF-8");
          }
        case "UTF-16BE":
          if(mb_strlen($hex)>3){
            echo(preg_replace_callback('|\\\\u([0-9a-f]{4})|i', function($matched){return mb_convert_encoding(pack('H*', $matched[1]), 'UTF-8', 'UTF-16');}, "\u".$hex));
            $hex="";
          }
          break;
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
  if($input){echo($input);}
?>
</textarea><br><br><br>
    <input type="submit" value="日本語化" name="ja">
    <input type="submit" value="ウェイ化" name="yo">
    <select name="enc">
      <option>UTF-8</option>
      <option>UTF-16LE</option>
      <option>UTF-16BE</option>
    </select>
<br><br><br>
output
    <textarea cols="40" rows="8">
<?php
  if($input){
    if($yo){
      encode($input,$enc);
    }elseif($ja){
      decode($input,$enc);
    }
  }
?>
</textarea>
  </form>
</body>
</html>