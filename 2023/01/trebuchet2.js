const { open } = require('node:fs/promises');

const wordMap = new Map();
wordMap.set("one", 1);
wordMap.set("two", 2);
wordMap.set("three", 3);
wordMap.set("four", 4);
wordMap.set("five", 5);
wordMap.set("six", 6);
wordMap.set("seven", 7);
wordMap.set("eight", 8);
wordMap.set("nine", 9);

const firstNumber = (str) =>
{
    if (str === "") return 0;
    let maybeNumber = Number(str.charAt(0));
    if (isNaN(maybeNumber))
    {
        for (const key of wordMap.keys())
        {
            if (str.startsWith(key))
            {
                return wordMap.get(key);
            }
        }
        return firstNumber(str.slice(1));
    }
    else
    {
        return maybeNumber;
    }
}

const lastNumber = (str) =>
{
    if (str === "") return 0;
    let maybeNumber = Number(str.slice(-1));
    if (isNaN(maybeNumber))
    {
        for (const key of wordMap.keys())
        {
            if (str.endsWith(key))
            {
                return wordMap.get(key);
            }
        }
        return lastNumber(str.slice(0, str.length - 1));
    }
    else
    {
        return maybeNumber;
    }
}




var result = 0;

(async () => {
  const file = await open('C:\\dev\\src\\nash\\advent\\20231201.1\\input-final');

  for await (const line of file.readLines()) {
    // var firstDigit = -1;
    // var lastDigit = -1;
    // let fixedLine = replaceWords(line)

    // for (let character of fixedLine) 
    // {
    //     let maybeNumber = Number(character);
    //     if (isNaN(maybeNumber) === false)
    //     {
    //         if (firstDigit < 0)
    //         {
    //             firstDigit = maybeNumber;
    //         }
    //         lastDigit = maybeNumber;
    //     }
    // }
    let firstDigit = firstNumber(line);
    let lastDigit = lastNumber(line);
    let lineVal =  (firstDigit * 10) + lastDigit;
    console.log(lineVal)
    result += lineVal
  }
  console.log(result);

})();

