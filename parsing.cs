﻿// Question:
// 1. In the parse_control, when should we consider clear_input() after Control is true? since we also have a buffer_input() happening at the same time.
// 2. 


////////// parsing function //////////////////
class buffer_input
{
    private int[] ele;
    private int front; // Beginning of the queue
    private int rear;  // End of the queue
    private int max;

    public queue(int size)
    {
        ele = new int[size];
        front = 0;
        rear = -1;
        max = size;
    }

    public void enqueue(char item)  // enqueue() --> insert an elelement to the queue
    {
        if (rear == max - 1)
        {
            Console.WriteLine("Queue Overflow");
            return;
        }
        else
        {
            ele[++rear] == item
        }
    }

    public char dequeue()  // dequeue() --> remove an element from the queue
    {
        if (front == rear + 1)
        {
            Console.WriteLine("Queue is Empty");
            return -1;
        }
        else
        {
            Console.WriteLine(ele[front] + " dequeued from queue");
            int p = ele[front++];
            Console.WriteLine();
            Console.WriteLine("Front item is {0}", ele[front]);
            Console.WriteLine("Rear item is {0}", ele[rear]);
            return p;
        }
    }

    public void printQueue()  // mainly to test out the queue
    {
        if (front == rear + 1)
        {
            Console.WriteLine("Queue is Empty");
            return;
        }
        else
        {
            for (int i = front; i <= rear, i++)
            {
                Console.WriteLine(ele[i] + " enqueued to queue");
            }
        }
    }

}

bool control;
Queue buffer = new Queue();
int i = 0;
char parse_escape(char N) // parse escape function
{
    string acl;
    char x;
    int code, id, channel, opcode, Value; // variables needed from the diagram
    bool no_escape, escape, escape_dot, reset_code, reset_code_digits, resource_id, resource_id_digits, temp_channel, // FSM state identifiers 
    temp_channel_digits, temp_opcode, temp_opcode_digits, temp_value, temp_value_digits, read_var_id, read_var_id_digits,
    write_var_id, write_var_id_digits, write_var_val, write_var_val_digits, start;


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
            parse_control(char N);
            return x;
        }
    }
    if (escape == true) // escape statement
    {
        if (x == '.' - '0')
        {
            escape_dot = true;
            escape = false;
            return x;
        }
        else
        {
            escape = false;
            start = true;
            parse_control(char N);
            return x;
        }
    }
    if (escape_dot == true) // escape_dot statement
    {
        if (x == 'E' - '0') // output 0 if there is an E
        {
            string send;
            send = "0\n\r";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'P' - '0' || x == 'Q' - '0') // output x0 if we have either P or Q
        {
            string send = new string;
            send = "x0\n\r";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'B' - '0') //escape dot B statement
        {
            char send[];
            buffer.CopyTo(send, 0);
            int temp = send.length();
            port.Write(send[], 0, temp);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'K' - '0') //escape dot K statement
        {
            buffer.clear();
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'I' - '0') //escape dot I statement
        {
            string send = new string;
            send = "x0,x0\n\r";
            port.Write(send);
            start = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'O' - '0') //escape dot O statement
        {
            //output extended status??
            escape_dot = false;
            return x;
        }
        else if (x == '!' - '0') // Reset code enable statement
        {
            reset_code = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'S' - '0') // resource id enable statement
        {
            resource_id = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'T' - '0') // temp channel enable statement
        {
            temp_channel = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'V' - '0') // read var id enable statement
        {
            read_var_id = true;
            escape_dot = false;
            return x;
        }
        else if (x == 'W' - '0') // write var id enable statement
        {
            write_var_id = true;
            escape_dot = false;
            return x;
        }
    }
    if (reset_code == true)
    {
        code = 0; // set code to 0
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            code = code * 10; // add_digits
            code = code + (int)x;
            reset_code = false;
            reset_code_digits = true;
            return x;
        }
        else
        {
            start = true;
            reset_code = false;
            parse_control(char N);
            return x;
        }
    }
    if (reset_code_digits == true)
    {
        if (code == '5' - '0')
        {
            //halt_procedure() we are not exactly sure what to do here
        }
        if (x == ':' - '0')
        {
            start = true;
            reset_code_digits = false;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            code = code * 10;
            code = code + (int)x;
            return x;
        }
        else
        {
            start = true;
            reset_code_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (resource_id == true)
    {
        id = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            resource_id = false;
            resource_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            resource_id = false;
            parse_control(char N);
            return x;
        }
    }
    if (resource_id_digits == true)
    {
        if (x == ':' - '0')
        {
            string send = "0\n\r";
            port.Write(send);
            resource_id_digits = false;
            start = true;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            return x;
        }
        else
        {
            start = true;
            resource_id_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (temp_channel == true)
    {
        channel = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            channel = channel * 10;
            channel = channel + (int)x;
            temp_channel_digits = true;
            temp_channel = false;
            return x;
        }
        else
        {
            start = true;
            temp_channel = false;
            parse_control(char N);
            return x;
        }
    }
    if (temp_channel_digits == true)
    {
        if (x == ';' - '0')
        {
            temp_opcode = true;
            temp_channel_digits = false;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            channel = channel * 10;
            channel = channel + (int)x;
            return x;
        }
        else
        {
            start = true;
            temp_channel_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (temp_opcode == true)
    {
        opcode = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            opcode = opcode * 10;
            opcode = opcode + (int)x;
            temp_opcode_digits = true;
            temp_opcode = false;
            return x;
        }
        else
        {
            start = true;
            temp_opcode = false;
            parse_control(char N);
            return x;
        }
    }
    if (temp_opcode_digits == true)
    {
        if (x == ';' - '0')
        {
            temp_value = true;
            temp_opcode_digits = false;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            opcode = opcode * 10;
            opcode = opcode + (int)x;
            return x;
        }
        else
        {
            start = true;
            temp_opcode_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (temp_value == true)
    {
        Value = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            Value = Value * 10;
            Value = Value + (int)x;
            temp_value_digits = true;
            temp_value = false;
            return x;
        }
        else
        {
            start = true;
            temp_value = false;
            parse_control(char N);
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
        if (x == ':' - '0')
        {
            start = true;
            temp_value_digits = false;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            Value = Value * 10;
            Value = Value + (int)x;
            return x;
        }
        else
        {
            start = true;
            temp_value_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (read_var_id == true)
    {
        id = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            read_var_id = false;
            read_var_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            read_var_id = false;
            parse_control(char N);
            return x;
        }
    }
    if (read_var_id_digits == true)
    {
        if (x == ':' - '0')
        {
            output_variable(id);
            read_var_id_digits = false;
            start = true;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            return x;
        }
        else
        {
            start = true;
            read_var_id_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (write_var_id == true)
    {
        id = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            write_var_id = false;
            write_var_id_digits = true;
            return x;
        }
        else
        {
            start = true;
            write_var_id = false;
            parse_control(char N);
            return x;
        }
    }
    if (write_var_id_digits == true)
    {
        if (x == ';' - '0')
        {
            write_var_val = true;
            write_var_id_digits = false;
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            id = id * 10;
            id = id + (int)x;
            return x;
        }
        else
        {
            start = true;
            write_var_id_digits = false;
            parse_control(char N);
            return x;
        }
    }
    if (write_var_val == true)
    {
        Value = 0;
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            Value = Value * 10;
            Value = Value + (int)x;
            temp_value_digits = true;
            write_var_val = false;
            return x;
        }
        else
        {
            start = true;
            write_var_val = false;
            parse_control(char N);
            return x;
        }
    }
    if (write_var_val_digits == true)
    {
        if (x == ':' - '0')
        {
            start = true;
            write_var_val_digits = false;
            //write variable into buffer id == location value == variable stored
            return x;
        }
        if (x >= '0' - '0' && x <= '9' - '0')
        {
            Value = Value * 10;
            Value = Value + (int)x;
            return x;
        }
        else
        {
            start = true;
            write_var_val_digits = false;
            parse_control(char N);
            return x;
        }
    }


}

char parse_control(char n)
{
    if (control == true)
    {
        if (n == '^' - '0')
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
    if (n == '^' - '0')
    {
        control = true;
        return n;
    }
    else if (n == '^')
    {
        control = false;
        buffer.Enqueue(n);
        return n;
    }
}

int output_variable(int l) // l = id 
{
    char temp[] = new char[];
    buffer.CopyTo(temp, 0);
    port.Write(temp[l], 0, 1);
    return l;
}
void write_variable(int l, int k) // we create a temporary char array that will hold the previous queue and then refill it with the updated que with the changed values
{  // the l = id and k = val in this case
    char temp[] = new char[];
    buffer.CopyTo(temp, 0)
  temp[l] = (char)k;
    foreach (char c in temp)
    {
        buffer.Enqueue(temp[i]);
    }
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
