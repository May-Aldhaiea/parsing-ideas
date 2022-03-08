// Question:
// 1. In the parse_control, when should we consider clear_input() after Control is true? since we also have a buffer_input() happening at the same time.
// 2. 


bool control;
Queue buffer = new Queue();
double varArray[1024];
string acl;
    char x;
    int code, id, channel, opcode; // variables needed from the diagram
    double Value;
    bool no_escape, escape, escape_dot, reset_code, reset_code_digits, resource_id, resource_id_digits, temp_channel, // FSM state identifiers 
    temp_channel_digits, temp_opcode, temp_opcode_digits, temp_value, temp_value_digits, read_var_id, read_var_id_digits,
    write_var_id, write_var_id_digits, write_var_val, write_var_val_digits, start = true;
int i = 0;
char parse_escape(char N) // parse escape function
{


    x = N;
    if (start == 1) // check if either the purple or the orange arrow statement been completly if so start should be true
    {
        if (x == 27) // escape sequence checker
        {
            escape = true;
            no_escape = false;
            start = false;
            return x; // ending any function with a return in order to not make one character go through multiple statements
        }
        else //purple arrow statement
        {
            no_escape = true;
            start = true;
            parse_control(N);
            return x;
        }
    }
    if (escape == true) // escape statement
    {
        if (x == '.')
        {
            escape_dot = true;
            escape = false;
            return x;
        }
        else
        {
            escape = false;
            start = true;
            parse_control(N);
            return x;
        }
    }
    if (escape_dot == true) // escape_dot statement
    {
        if (x == 'E') // output 0 if there is an E
        {
            string send;
            send = "0\r\n";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'P' || x == 'Q') // output x0 if we have either P or Q
        {
            string send = new string;
            send = "x0\r\n";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'B') //escape dot B statement
        {
            char send[];
            buffer.CopyTo(send, 0);
            int temp = send.length();
            port.Write(send[], 0, temp);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'K') //escape dot K statement
        {
            buffer.clear();
            start = true;
            escape_dot = false;
            control = false;
            return x;
        }
        else if (x == 'I') //escape dot I statement
        {
            string send = new string;
            send = "x0,x0\n\r";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'O') //escape dot O statement
        {
            //output extended status??
            escape_dot = false;
            start = true;
            return x;
        }
        else if (x == '!') // Reset code enable statement
        {
            reset_code = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'S') // resource id enable statement
        {
            resource_id = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'T') // temp channel enable statement
        {
            temp_channel = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'V') // read var id enable statement
        {
            read_var_id = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'W') // write var id enable statement
        {
            write_var_id = true;
            escape_dot = false;
            return x;
        }
    }
    if (reset_code == true)
    {
        code = 0; // set code to 0
        if (x >= '0' && x <= '9')
        {
            code = code * 10; // add_digits
            code = code + (int)(x- '0');
            reset_code = false;
            reset_code_digits = true;
            return x;
        }
        else
        {
            start = true;
            reset_code = false;
            parse_control(N);
            return x;
        }
    }
    if (reset_code_digits == true)
    {
        if (code == '5')
        {
            //halt_procedure() we are not exactly sure what to do here
        }
        if (x == ':')
        {
            start = true;
            reset_code_digits = false;
            return x;
        }   
        if (x >= '0' && x <= '9')
        {
            code = code * 10;
            code = code + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            reset_code_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (resource_id == true)
    {
        id = 0;
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            resource_id = false;
            resource_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            resource_id = false;
            parse_control(N);
            return x;
        }
    }
    if (resource_id_digits == true)
    {
        if (x == ':')
        {
            string send = "0\n\r";
            port.Write(send);
            resource_id_digits = false;
            start = true;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            resource_id_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_channel == true)
    {
        channel = 0;
        if (x >= '0' && x <= '9' )
        {
            channel = channel * 10;
            channel = channel + (int)(x- '0');
            temp_channel_digits = true;
            temp_channel = false;
            return x;
        }
        else
        {
            start = true;
            temp_channel = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_channel_digits == true)
    {
        if (x == ';')
        {
            temp_opcode = true;
            temp_channel_digits = false;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            channel = channel * 10;
            channel = channel + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            temp_channel_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_opcode == true)
    {
        opcode = 0;
        if (x >= '0' && x <= '9')
        {
            opcode = opcode * 10;
            opcode = opcode + (int)(x- '0');
            temp_opcode_digits = true;
            temp_opcode = false;
            return x;
        }
        else
        {
            start = true;
            temp_opcode = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_opcode_digits == true)
    {
        if (x == ':')
        {
            temp_value = true;
            temp_opcode_digits = false;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            opcode = opcode * 10;
            opcode = opcode + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            temp_opcode_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_value == true)
    {
        Value = 0;
        if (x >= '0' && x <= '9')
        {
            Value = Value * 10;
            Value = Value + (int)(x- '0');
            temp_value_digits = true;
            temp_value = false;
            return x;
        }
        else
        {
            start = true;
            temp_value = false;
            parse_control(N);
            return x;
        }
    }
    if (temp_value_digits == true)
    {
        if (opcode == 2)
        {
            string send = "0.0\n\r";
            port.Write(send);
            return x;
        }
        else if (opcode == 4 || opcode == 43)
        {
            string send = "0\n\r";
            port.Write(send);
            return x;
        }
        else if (opcode == 42)
        {
            string send = "LCO\n\r";
            string send1 = "HCO\n\r";
            port.Write(send);
            port.Write(send1);
            return x;
        }
        if (x == ':')
        {
            start = true;
            temp_value_digits = false;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            Value = Value * 10;
            Value = Value + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            temp_value_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (read_var_id == true)
    {
        id = 0;
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            read_var_id = false;
            read_var_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            read_var_id = false;
            parse_control(N);
            return x;
        }
    }
    if (read_var_id_digits == true)
    {
        if (x == ':')
        {
            output_variable(id);
            read_var_id_digits = false;
            start = true;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            read_var_id_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (write_var_id == true)
    {
        id = 0;
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            write_var_id = false;
            write_var_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            write_var_id = false;
            parse_control(N);
            return x;
        }
    }
    if (write_var_id_digits == true)
    {
        if (x == ';')
        {
            write_var_val = true;
            write_var_id_digits = false;
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            id = id * 10;
            id = id + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            write_var_id_digits = false;
            parse_control(N);
            return x;
        }
    }
    if (write_var_val == true)
    {
        Value = 0;
        if (x >= '0' && x <= '9')
        {
            Value = Value * 10;
            Value = Value + (int)(x- '0');
            temp_value_digits = true;
            write_var_val = false;
            return x;
        }
        else
        {
            start = true;
            write_var_val = false;
            parse_control(N);
            return x;
        }
    }
    if (write_var_val_digits == true)
    {
        if (x == ':')
        {
            start = true;
            write_var_val_digits = false;
            write_variable(id,Value);
            return x;
        }
        if (x >= '0' && x <= '9')
        {
            Value = Value * 10;
            Value = Value + (int)(x- '0');
            return x;
        }
        else
        {
            start = true;
            write_var_val_digits = false;
            parse_control(N);
            return x;
        }
    }


}

char parse_control(char n)
{
    if (control == true)
    {
        if (n == '^')
        {
            buffer.Enqueue(n);
            control = false;
            return n;
        }
        else
        {  // we are not shower what is the requirement for clearing the input yet
            buffer.Enqueue(n & 31);
            control = false;
            return n;
        }
    }
    if (n == '^')
    {
        control = true;
        return n;
    }
    else if (n != '^')
    {
        control = false;
        buffer.Enqueue(n);
        return n;
    }
}

void output_variable(int l) // l = id 
{
    double temp = varArray[l];
    port.Write($"{temp:F}\r\n");
}
void write_variable(int l, double k) // we create a temporary char array that will hold the previous queue and then refill it with the updated que with the changed values
{  // the l = id and k = val in this case
    varArray[l] = k;
}


////////////////////////////////////////////////

{
    // if stage five is done and ready to give the substrate to the board
    string command = "1";
    Write(command[] = "1", 0, 1);
    while (ReadExisting() == 1)
        Thread.Sleep(1);
    // else if the substrate is not ready 
    string command = "0";
    Write(command[] = "1", 0, 1);
    while (ReadExisting() == 1)
        Thread.Sleep(1);
}
}

///////////////////////////////////////////////////////
