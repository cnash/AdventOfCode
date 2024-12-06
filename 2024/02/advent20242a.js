const inputFile = "G:\\My Drive\\code\\adventofcode\\2024\\2\\input\\actual"

const fs = require('node:fs');

fs.readFile(inputFile, 'utf8', (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  let total = 0;
  let lines = data.split('\r\n');

  for (var i in lines)
  {
    let vals = lines[i].split(" ").map((x)=>Number(x));
    let val1=-1;
    let val2=-1;
    let increasing = undefined;
    do {
        val1 = val2;
        val2 = vals.pop();
        if (val1<0) continue;
        if (val2 === undefined)
        {
            total++;
            break;
        }
        if (increasing === undefined)
        {
          increasing = val2 > val1;
        }
        else if (increasing !== (val2 > val1))
        {
          break;
        }
        let diff = Math.abs(val1-val2);
        if (diff > 3 || diff == 0) break;

    } while (true);
  }
  console.log(total);
});
