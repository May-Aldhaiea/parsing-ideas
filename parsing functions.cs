char readChar;
int ascii;
bool escSequence = false, commandSequence = false;
void fixString()
{
  vantage.readTo(readVantage);
  //readVantage = vantage.readLine();
  foreach (char c in readVantage)
  {
    ascii = c - '0';
    if(ascii >= 33 && ascii <= 122)
    {
      if (ascii == 45 || ascii == 95)
      {
      }else if(ascii >= 97 && ascii <= 122)
      {
        readChar = char.ToUpper(c);
      }
      else
      {
        readChar = c;
      }
    }
    if (ascii == 27)
    {
      readChar = c;
      parse_escape(readChar);
      escSequence = true;
    }
    if (escSequence == true)
    {
      parse_escape(readChar);
    }else
    {
      command_loop();
      commandSequence = true;
    }
    if (start == true)
    {
      escSequence = false;
    }
    if (commandSequence = true)
    {
      command_loop();
    }
    if (B = BD = D = DO = DI = E = ED = I = II = IN = IO = O = OA = 
      OC = OD = OE = OF = OI = ON = OO = OP = OS = R = RA = S = 
      SA = T = TC = V = VC = VS = X = XD = false)
    {
      commandSequence = false;
    }
  }
}