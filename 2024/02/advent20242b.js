const inputFile = "G:\\My Drive\\code\\adventofcode\\2024\\2\\input\\actual"

const fs = require('node:fs');

fs.readFile(inputFile, 'utf8', (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  let lines = data.split('\r\n');

  const isSafe = (vals) => {
    let val1=-1;
    let val2=-1;
    let increasing = undefined;

    do {
        val1 = val2;
        val2 = vals.pop();
        if (val1<0) continue;
        if (val2 === undefined)
        {
           return true;
        }
        let bigger = val2 > val1;
        if (increasing === undefined)
        {
          increasing = bigger;
        }
        if (increasing !== bigger)
        {
          return false
        }
        let diff = Math.abs(val1-val2);
        if (diff > 3 || diff == 0)
        {
          return false;
        }

    } while (true);
  }


  let total = lines.filter(line => {
      let vals = line.split(" ").map((x)=>Number(x));
      for (var i in vals)
      {
        if (isSafe(vals.toSpliced(i, 1))) return true;
      }
      return false;
    }
  ).length;
  console.log(total);
});
