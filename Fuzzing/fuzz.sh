#!/bin/bash

FORMAT[0]="bytes"
FORMAT[1]="hex"
FORMAT[2]="int"
FORMAT[3]="bits"
FORMAT[4]="array"


fuzz() { 
    for ((it=0; it < $1; it++)) 
    do 
        for ((i=0; i <= 4 ; i++))
        do
        	for ((j=0; j <= 4 ; j++))
        	do
                if [[ $i -eq 0 ]]; then
                    head -c 32  /dev/urandom | radamsa >  "input${FORMAT[0]}"
                fi

                if [[ $i -eq 1 ]]; then
                    echo '12345678910abcdeffffa' | radamsa > "input${FORMAT[1]}"
                fi

                if [[ $i -eq 2 ]]; then
                    echo '1234000 10' | radamsa > "input${FORMAT[2]}"
                fi

                if [[ $i -eq 3  ]]; then
                    echo '111000111011010' | radamsa > "input${FORMAT[3]}"
                fi

                if [[ $i -eq 4 ]]; then
                    echo "{0x1, 0x2, 0x3, 0x4}" | radamsa > "input${FORMAT[4]}"
                fi


                dotnet run --project Panbyte --  -f "${FORMAT[$i]}" -t "${FORMAT[$j]}" -i "input${FORMAT[$i]}" -o "output${FORMAT[$i]}"
        	done
        done
    done
}

fuzz "$1"
