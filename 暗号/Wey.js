var WeySoiya = function () {
    this.wey2 = new Array("うぇい", "そいや", "ウェイ", "ソイヤ");
    this.REwey2 = new Array();
    for (var i = 0; i < this.wey2.length; i++) {
        this.REwey2[i] = new RegExp(this.wey2[i], "g");
    }
    this.wey4 = new Array("ウェイ", "ソイヤ", "うぇい", "そいや", "ウェい", "ソイや", "ウぇイ", "ソぃヤ", "ウぇい", "ソいや", "うェイ", "そイヤ", "うぇイ", "そいヤ", "うェい", "そイや");
    this.REwey4 = new Array();
    for (var i = 0; i < this.wey4.length; i++) {
        this.REwey4[i] = new RegExp(this.wey4[i], "g");
    }
    this.grass = new Array("w", "W");
    this.REgrass = new Array();
    for (var i = 0; i < this.grass.length; i++) {
        this.REgrass[i] = new RegExp(this.grass[i], "g");
    }
    this.MM = new Array("頑張ります!", "それはそういうものです","後でじっくり話しましょう","谷川ァ…");
    this.REMM = new Array();
    for (var i = 0; i < this.MM.length; i++) {
        this.REMM[i] = new RegExp(this.MM[i], "g");
    }
    this.code = function (string, type) {
        if (string.length == 0) {
            document.getElementById("output").innerHTML = "なにか文字入れろゴミカス";
            return false;
        }
        var int = new Array();
        var Key = new Array();
        var REKey = new Array();
        var num;
        if (type == 0) {
            num = 2;
            Key = this.wey2.slice();
            REKey = this.REwey2.slice();
        }
        else if (type == 1) {
            num = 4;
            Key = this.wey4.slice();
            REKey = this.REwey4.slice();
        }
        else if (type == 3) {
            num = 1;
            Key = this.grass.slice();
            REKey = this.REgrass.slice();
        }
        else if (type == 4) {
            num = 2;
            Key = this.MM.slice();
            REKey = this.REMM.slice();
        }
        document.getElementById("message").innerHTML = "変換中…";
        var i = 0, timer = setInterval(function () {
            int[i] = string.slice(i, i + 1);
            if (int[i] == "\n") {
                //改行の処理
                int[i] = mypow(Key[0], num);
                document.getElementById("output").innerHTML += int[i];
            }
            else {
                int[i] = escape(int[i]);
                if (int[i].indexOf("%u") != -1) {
                    //普通のユニコード文字
                    int[i] = (mypow("0", num) + (parseInt(int[i].replace("%u", ""), 16).toString(Math.pow(2, num)))).slice(-16 / num);
                }
                else if (int[i].indexOf("%") != -1) {
                    //半角カッコとか
                    int[i] = (mypow("0", num) + (parseInt(int[i].replace("%", ""), 16).toString(Math.pow(2, num)))).slice(-16 / num);
                }
                else {
                    //半角英数字
                    int[i] = (mypow("0", num) + (int[i].charCodeAt(0).toString(Math.pow(2, num)))).slice(-16 / num);
                }
                for (var j = 0; j < Key.length; j++) {
                    int[i] = int[i].replace(new RegExp(j.toString(16), "g"), Key[j]);
                }
                document.getElementById("output").innerHTML += int[i];
            }
            i++;
            if (i >= string.length) {
                document.getElementById("outp_length").innerHTML =
                    document.getElementById("output").innerHTML.length;
                document.getElementById("message").innerHTML = "変換完了";
                clearInterval(timer);
            }
        }, 0);
    }
    this.decode = function (string, type) {
        if (string.length == 0) {
            document.getElementById("output").innerHTML = "なにか文字入れろゴミカス";
            return false;
        }
        var int = new Array();
        var Key = new Array();
        var REKey = new Array();
        var num;
        if (type == 0) {
            num = 2;
            Key = this.wey2.slice();
            REKey = this.REwey2.slice();
        }
        else if (type == 1) {
            num = 4;
            Key = this.wey4.slice();
            REKey = this.REwey4.slice();
        }
        else if (type == 3) {
            num = 1;
            Key = this.grass.slice();
            REKey = this.REgrass.slice();
        }
        else if (type == 4) {
            num = 2;
            Key = this.MM.slice();
            REKey = this.REMM.slice();
        }
        document.getElementById("message").innerHTML = "変換中…";
        var i = 0, timer = setInterval(function () {
            int[i] = string.slice(Key[0].length * 16 / num * i, Key[0].length * 16 / num * (i + 1));
            for (var j = 0; j < REKey.length; j++) {
                int[i] = int[i].replace(REKey[j], j.toString(16));
            }
            if (int[i] == mypow("0", num)) {
                //改行
                int[i] = "<br>";
                document.getElementById("output").innerHTML += int[i];
            }
            else {
                int[i] = parseInt(int[i], Math.pow(2, num)).toString(16);
                int[i] = unescape("%u" + ("0000" + int[i]).slice(-4));
                document.getElementById("output").innerHTML += int[i];
            }
            i++;
            if (i >= string.length / Key[0].length / 16 * num) {
                document.getElementById("outp_length").innerHTML =
                    document.getElementById("output").innerHTML.length;
                document.getElementById("message").innerHTML = "変換完了";
                clearInterval(timer);
            }
        }, 0);
    }
}
function mypow(string, num) {
    var result = "";
    for (var j = 0; j < 16 / num ; j++) {
        result += string;
    }
    return result;
}