const { open } = require('node:fs/promises');

var result = 0;

(async () => {
  const file = await open('C:\\dev\\src\\nash\\advent\\20231201.1\\input-final');

  for await (const line of file.readLines()) {
    var firstDigit = -1;
    var lastDigit = -1;

    for (let character of line) 
    {
        let maybeNumber = Number(character);
        if (isNaN(maybeNumber) === false)
        {
            if (firstDigit < 0)
            {
                firstDigit = maybeNumber;
            }
            lastDigit = maybeNumber;
        }
    }
    result += (firstDigit * 10) + lastDigit;
  }
  console.log(result);

})();
