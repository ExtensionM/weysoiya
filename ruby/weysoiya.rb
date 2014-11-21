# weysoiya converter

#initilaize 
i = 0
@array = ["ウェイ","ソイヤ","うぇい","そいや",
		 "ウェい","ソイや","ウぇイ","ソぃヤ",
		 "ウぇい","ソいや","うェイ","そイヤ",
		 "うぇイ","そいヤ","うェい","そイや"]

puts "Please Input Some Words"
#get words 
text = gets.chomp
def encode(text)
  text = text.encode("UTF-8")
	#文字ごとに文字コードを取得
  text.each_codepoint { |cp| 
    #16進数に変換
	 	code = cp.to_s(16)
		#16進数に変換したものを一桁ずつweysoiyaに変換する
		while i < code.length 
	  	print array[code[i].to_i(16)]
	  	i+=1
		end
		i = 0
  }
end

def decode(text)
  text.encode("UTF-8")
  l = 0
	number = ""
	keta = 	text.length / 3 - 1 
	count = 0
  while l < text.length
		#もし残りの文字数が三文字以下ならbreak
		if text.length - l < 3
			break
		end
		str = ""
		#3文字で1~fを表すので三文字分をまず取得
		3.times do
      str += text[l]
			l += 1
    end  	
		#3文字ずつ取得したweysoiyaを16進数に変換する
		if @array.index(str) != nil
			number += @array.index(str).to_s(16)
		end
		keta -= 1
		count += 1
		
		if count % 4 == 0
		  print number.to_i(16).chr("UTF-8")
			number = ""
    end
	end
end
decode(text)
