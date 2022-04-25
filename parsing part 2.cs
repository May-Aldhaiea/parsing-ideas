
bool B, BD, D, DO, DI, E, ED, I, II, IN, IO, O, OA, OC, OD, OE, OF, OI, ON, OO, OP, OS, R, RA, S, SA, T, TC, V, VC, VS, X, XD;
int id1, VCoutput;
string outputID, convert;
char command_loop()
{
  char h;
  buffer.TryDequeue(out h);
  
  /*if (buffer.IsEmpty)
  {
    B = BD = D = DO = DI = E = ED = I = II = IN = IO = O = OA = 
      OC = OD = OE = OF = OI = ON = OO = OP = OS = R = RA = S = 
      SA = T = TC = V = VC = VS = X = XD = false;
  }*/
  if (D == true)
  {
  	switch(h)
      {
        case "I":
        	vantage.Write{"0\r\n"}
        	D = false;
         	DI = true;
      		break;
        case "O":
       		D = false;
          	DO = true;
        	break;
      }
    return h;
  }
  if (E == true)
  {
    switch(h)
    {
      case "D":
        E = false;
        // cancel downloading mode
        break;
    }
    return h;
  }
  if (I == true)
  {
    switch(h)
    {
      case "I":
    		vantage.Write{"0 x0\r\n"};
        I = false;
        break;
      case "N":
        I = false;
        // clear ACL error code?
        // OS status bit cleared?
        break;
      case "O":
        I = false;
        IO = true;
        break;
    }
  }
  
  // "O" command
	if(O == true)
  {
    switch(h)
    {
      case "A":
        vantage.Write{"0,0\r\n"};
        O = false;
        break;
      case "C":
        vantage.Write{"0,0\r\n"};
        O = false;
        break;
      case "D":
        vantage.Write{"0 x0\r\n"};
        O = false;
        break;
      case "E":
        // ACL error code
        O = false;
        break;
      case "F":
        vantage.Write{"0,0\r\n"};
        O = false;
        break;
      case "I":
        // vantage handler outputing "CCX-BLD9999Teleporter"
        vantage.Write{"CCX-BLD9999Teleporter\r\n"};
        O = false;
        break;
      case "N":
        vantage.Write{"0 x0\r\n"};
        O = false;
        break;
      case "O": 
        vantage.Write{"0,0\r\n"};
        O = false;
        break;
      case "P":
        // selector
        vantage.Write{"0\r\n"};
        O = false;
        break;
      case "S";
        // decimal integer in the range 0 through 1023
        int code;
        if (code == 8)
        O = false;
        break;
      // case "V";
      //   // varid with a bit size of 31
      //   break;
        
    }
  }
    
  if (R == true)
  {
    if (h == "A")
    {
      RA = true;
      R = false;
    }
    return h;
  }
  if (S == true)
  {
    if (h == "A")
    {
      S = false;
      SA = true;
    }
    return h;
  }
  if (T == true)
  {
    if (h == "C")
    {
      TC = true;
      T = false;
    }
    return h;
  }
  if (V == true)
  {
    if (h == "C")
    {
      VC = true;
      V = false;
    }else if (h == "S")
    {
      VS = true;
      V = false;
    }
    return h;
  }
  if (X == true)
  {
    if (h == "D")
    {
      X = false;
      XD = true;
    }
    return h;
  }
  if (XD == true)
  {
    XD = false;
    switch(id)
    {
      case 0:
        break;
      case 1:
        break;
      case 2:
        break;
      case 3:
        break;
      case 4:
        break;
      case 0:
        break;
      case 5:
        break;
      case 6:
        break;
      case 7:
        break;
      case 8:
        break;
      case 9:
        break;
      case 10:
        break;
    }
    return h;
  }
  if (TC == true)
  {
    if (opcode == 2)
    {
      vantage.Write{"0.0\r\n"};
    }else if (opcode == 42)
    {
      vantage.Write{"LCO\r\n"};
      vantage.Write{"HCO\r\n"};
    }else if (opcode == 4 || opcode == 43)
    {
      vantage.Write{"0\r\n"};
    }
    TC = false;
    return h;
  }
  if (VC == true)
  {
    // capture output identifcation
    convert = Regex.Match(outputID, @"\d+").Value;
    VCoutput = Int32.Parse(convert);
    VC = false;
    return h;
  }
  if (VS == true)
  {
    
  }
  if (SA == true)
  {
     if (/* operation is omitted */)
     {
       SA = false;
       vantage.Write{"0 x0\r\n"};
     }
    return h;
  }
  if (RA == true)
  {
    // chan samples?
    vantage.Write{"0\r\n"};
    RA = false;
    return h;
  }
  
  if (IO == true)
  {
    if(h == "-1")
    {
      	vantage.Write{"0 x0\r\n"};
    }
    return h;
  }

  }
  if (DI == true)
  {
    // This command reads the status of the digital bit number specified
	if (h == ';')
	{
		vantage.Write{"0\r\n"}
		DI = false;
	}
    return h;
  }
  if (DO == true)
  {
    /*if (h == "1")
    {
      // outputs shadow registers to a host computer in decimal
      vantage.Write{"0,0\r\n"}
      return h;
    }*/
    // outputs shadow registers to a host in hexadecimal 
    if (h == ';')
	{
		vantage.Write{"0,o\r\n"}
		DO = false;
	}
    return h;
  }
  switch(h)
  {
    case "B":
      B = true;
      break;
    case "D":
      D = true;
      break;
    case "E":
      E = true;
      break;
    case "I":
      I = true;
      break;
    case "O":
      O = true;
      break; 
    case "R":
      R = true;
      break;
    case "S":
      S = true;
      break;
    case "T":
      T = true;
      break;
    case "V":
      V = true;
      break;
    case "X":
      X = true;
      break;
  }
  return h;
}
