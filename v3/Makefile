
test: Bowling.fs spoj
	sed -n -e 's/\(<- \)\(.*\)/\2/pw input.dat'     test.txt >/dev/null
	sed -n -e 's/\(-> \)\(.*\)/\2/pw temp.dat'      test.txt >/dev/null
	sed 's/\(.*\)/\1 /pw expected.dat'              temp.dat >/dev/null
	gforth-itc spoj.fs <input.dat >result.dat
	diff expected.dat result.dat

unit: Bowling.fs tests.fs
	gforth-itc tests.fs

spoj: Bowling.fs
	echo "MAIN BYE" >main.fs
	cat Bowling.fs main.fs >spoj.fs

clean : 
	rm *.tmp ; rm *.dat
