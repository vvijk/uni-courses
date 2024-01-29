echo "*** starting build of parser demo"

echo "*** compiling each module"

gcc -c -Wall Dlexer.c
gcc -c -Wall Dsymtab.c
gcc -c -Wall Dkeytoktab.c
gcc -c -Wall Doptab.c

gcc -c -Wall driver.c

gcc -c -Wall lexer.c
gcc -c -Wall parser.c
gcc -c -Wall symtab.c
gcc -c -Wall keytoktab.c
gcc -c -Wall optab.c

echo "*** End of separate module compilation"

echo "*** Build and test the driver programs"

gcc  -o tlexer Dlexer.c     keytoktab.c    lexer.c
gcc  -o ttok   Dkeytoktab.c keytoktab.c
gcc  -o tsym   Dsymtab.c    keytoktab.c    symtab.c
gcc  -o top    Doptab.c     keytoktab.c    optab.c

echo "*** testing the driver programs"

echo "Saving previous output"
cp Dlexer.out Dlexer.old
cp Dtoktab.out Dtoktab.old
cp Dsymtab.out Dsymtab.old
cp Doptab.out Doptab.old

echo "*** Running tests"

./tlexer < testok1.pas > Dlexer.out
./ttok   > Dtoktab.out
./tsym   > Dsymtab.out
./top    > Doptab.out

echo "*** Comparing current and previous outputs "
echo "*** Lexer output"
diff Dlexer.old Dlexer.out
echo "*** Keytoktab output"
diff Dtoktab.old Dtoktab.out
echo "*** Symtab output"
diff Dsymtab.old Dsymtab.out
echo "*** Optab output"
diff Doptab.old Doptab.out
echo "*** End of comparison"

echo "*** end of driver tests"

echo "*** Building Parser"

gcc -o parser driver.c lexer.c parser.c keytoktab.c symtab.c optab.c

echo "*** Running parser test (testok1.pas)"

./parser < testok1.pas

echo "*** End of parser test"

echo "*** Running all tests (testall.sh)"
echo "*** Saving previous output"
cp testall.out testall.old
echo "*** Running tests"
./testall.sh > testall.out
echo "*** Comparing outputs"
diff testall.old testall.out
echo "*** End of all tests"



