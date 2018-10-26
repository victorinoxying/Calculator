using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace Calculator
{
   
    public partial class Background : Form
    {
        public Background()
        {
           
            InitializeComponent();
        }
        
        //储存计算使用的string表达式
        public string expression = "";
        
        //返回最后一个操作
        public string Current(string s)
        {
            string current = "";
            string[] list = s.Split(' ');
            if (list[list.Length - 1].Length == 0)
            {
                //防止分词后储存了空单位
                for (int i = list.Length - 2; i >= 0; i--)
                {
                    if (list[i].Length > 0)
                    {
                        current = list[i];
                        break;
                    }
                }
            } 
            else
                current = list[list.Length - 1];
            return current;
        }
        //判断否是特殊运算符
        public bool IsSpecoalSign(string str)
        {
            bool sign = false;
            switch (str)
            {
                case "cos":
                case "sin":
                case "tan":
                case "√":
                case "ln":
                case "lg":
                case "sinh":
                case "cosh":
                case "tanh":
                    sign = true;
                    break;
                default:
                    break;
            }
            return sign;
        }

        //正则表达式来判断一个字符串是否为数字
        public static bool IsNumeric(string strNumber)
        {
            Regex IsInt = new Regex("^-?\\d+$");
            Regex IsFloat = new Regex("^(-?\\d+)(\\.\\d+)?$");
            return IsInt.IsMatch(strNumber)||IsFloat.IsMatch(strNumber);
                   
        }

        void loopPushOperator(string s, string oper, Stack<string> operators)
        {
            if (operators.Count > 0 && Priority(oper)<=Priority(operators.Peek()))
            {
                s += operators.Peek()+"　";
                operators.Pop();
                loopPushOperator(s, oper, operators);
            }
        }


        //判断优先级
        public int Priority(string oper)
        {
            int priority = 0;
            switch (oper) {
                case "+":
                case "-":
                    priority = 1; break;
                case "*":
                case "/":
                    priority = 2;break;
                case "cos":
                case "sin":
                case "tan":
                case "√":
                case "ln":
                case "lg":
                case "sinh":
                case "cosh":
                case "tanh":
                case "!":
                    priority = 3;break;
                case "^":
                case "x√":
                    priority = 4; break;
                case ")":
                    priority = 5;break;
 
            }
            return priority;
        }
       

        //根据运算符,计算栈顶两个数的值,并将计算的值压栈
        void CalculateResult(string oper, Stack<double> tmpStack)
        {
            if (tmpStack.Count < 2)
            {
                return;
            }
            //栈是先进后出,所以先弹出的是第二个值
            double secondVal = tmpStack.Peek();
            tmpStack.Pop();
            double firstVal = tmpStack.Peek();
            tmpStack.Pop();

            double result = 0;
            switch (oper)
            {
                case "+":
                    result = firstVal + secondVal;
                    break;
                case "-":
                    result = firstVal - secondVal;
                    break;
                case "*":
                    result = firstVal * secondVal;
                    break;
                case "/":
                    result = firstVal / secondVal;
                    break;
                case "√":
                    result = firstVal * Math.Sqrt(secondVal);//first值是补位左值 1
                    break;
                case "cos":
                    result = firstVal * Math.Cos(secondVal);
                    break;
                case "sin":
                    result = firstVal * Math.Sin(secondVal);
                    break;
                case "tan":
                    result = firstVal * Math.Sinh(secondVal);
                    break;
                case "sinh":
                    result = firstVal * Math.Tanh(secondVal);
                    break;
                case "cosh":
                    result = firstVal * Math.Cosh(secondVal);
                    break;
                case "tanh":
                    result = firstVal * Math.Tanh(secondVal);
                    break;
                case "ln":
                    result = firstVal * Math.Log(secondVal);
                    break;
                case "lg":
                    result = firstVal * Math.Log10(secondVal);
                    break;
                case "!":
                    result = secondVal * Factorical(firstVal);
                    break;
                case "^":
                    result = Math.Pow(firstVal, secondVal);
                    break;
                case "x√":
                    result = Math.Pow(secondVal, 1 / firstVal);
                    break;
                default:
                    break;
            }

            tmpStack.Push(result);
        }

        //阶乘的计算函数
        public double Factorical(double num)
        {
            if (num < 1)
                return 0;
            double result = 1;
            while (Math.Floor(num) > 0)
            {
                result = result * num;
                num--;
            }
            return result;
        }

        //中缀表达式转后缀表达式
        string infixToPostfix(string s)
        {
            Stack<string> operators = new Stack<string>();     //运算符栈
            string Postfix = "";                        //后缀表达式
            string[] inexpression = s.Split(' ');
            
            for (int i = 0; i < inexpression.Length; i++)
            {
                
                if (IsNumeric(inexpression[i]))
                {   //如果是数字直接加到后缀表达式尾
                    Postfix += inexpression[i]+" ";
                }
                else
                {
                    switch (inexpression[i])
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                        case "^":
                        case "√":
                        case "cos":
                        case "sin":
                        case "tan":
                        case "ln":
                        case "lg":
                        case "x√":
                        case "sinh":
                        case "cosh":
                        case "tanh":
                        case "!":
                            //如果运算符栈为空, 直接将当前运算符压栈
                            if (operators.Count <= 0)
                            {
                                operators.Push(inexpression[i]);

                            }
                            //如果当前运算符优先级小于等于栈顶运算符优先级, 将栈顶运算符加到后缀表达式尾
                            else if (Priority(inexpression[i]) <= Priority(operators.Peek()))
                            {
                                Postfix += operators.Peek() + " ";
                                operators.Pop();
                                while(operators.Count>0&&Priority(inexpression[i]) <= Priority(operators.Peek()))
                                {
                                    
                                    Postfix += operators.Peek() + " ";
                                    operators.Pop();
                                    
                                }
                                operators.Push(inexpression[i]);

                            }
                            //如果当前运算符优先级大于栈顶运算符优先级, 将当前运算符压栈
                            else 
                            {
                                operators.Push(inexpression[i]);
                            }
                            break;
                        case "(":
                            operators.Push(inexpression[i]);  //当前运算符为"("直接压栈
                            break;
                        case ")":
                            //将栈中元素弹出加到后缀表达式尾,直到遇到运算符"("
                            while (operators.Peek()!="(")
                            {
                                Postfix += operators.Peek() + " ";
                                operators.Pop();
                            }
                            operators.Pop();
                            break;
                        default:
                            break;
                    }  //处理运算符
                }
            }

            if (Postfix.Length > 0)
            {
                while (operators.Count > 0)
                {  //将运算符栈中留有的运算符全部出栈加到后缀表达式尾
                    Postfix += operators.Peek()+" ";
                    operators.Pop();
                }
                return Postfix;
            }
            else
            {
                return "";
            }
        }

     

        /* 通过后缀表达式计算结果
         * 将后缀表达式依次入栈, 如果为操作符, 弹出栈中两个元素计算结果再压入栈中
         */
        double getResultUsePostfix(string s)
        {
            if (s.Length <= 0)
            {
                return 0;
            }
            string[] postexpression = s.Split(' ');
            Stack<double> tmpStack = new Stack<double>();
            for (int i = 0; i < postexpression.Length; i++)
            {
                if (IsNumeric(postexpression[i]))//如果是数字直接压入栈
                {
                    tmpStack.Push(Convert.ToDouble(postexpression[i]));
                }
                else
                {
                    CalculateResult(postexpression[i], tmpStack);//用计算符号计算栈内数字
                }
            }
            if (tmpStack.Count > 0)
            {
                return tmpStack.Peek();//返回计算值
            }
            else return 0;
            
        }
        //每个按键响应后，会输入相应数值到两个表达式中，一个用于运算，数字和符号用“ ”分割，一个用于展示给用户
        private void button_num0_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "0";
            expression += "0";

        }

        private void button_num1_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "1";
            expression += "1";
            
        }

        
        private void button_num2_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "2";
            expression += "2";

        }

        private void button_num3_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "3";
            expression += "3";

        }

        private void button_num4_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "4";
            expression += "4";

        }

        private void button_num5_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "5";
            expression += "5";

        }

        private void button_num6_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "6";
            expression += "6";

        }

        private void button_num7_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "7";
            expression += "7";

        }

        private void button_num8_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "8";
            expression += "8";

        }

        private void button_num9_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "9";
            expression += "9";

        }
        //删除键
        private void button_DEL_Click(object sender, EventArgs e)
        {
            if (expression.Length > 0&&this.theProgress.Text.Length>0)
            {
                string last = Current(expression);
                if (IsSpecoalSign(last))
                {
                    expression = expression.Remove(expression.LastIndexOf(" 1 " + last));
                }
                else
                {
                   //对括号的处理，删除左一个括号，括号的计数器减一，删除一个右括号，括号的计数器加一
                    if (last.Equals(")"))
                        right_Parenthesis_number++;
                    if (last.Equals("("))
                        right_Parenthesis_number--;
                    expression = expression.Remove(expression.LastIndexOf(last));
                }
                if(this.theProgress.Text.Contains(last))
                    this.theProgress.Text = this.theProgress.Text.Remove(this.theProgress.Text.LastIndexOf(last));
                Is_Equalsign_Clicked = false;
                Memory_flag = false;
            }
            

        }

        private void button_AC_Click(object sender, EventArgs e)//复位键，将所有flag和文本框归零
        {
            this.expression = "";
            this.theProgress.Text = "";
            this.theResult.Text = "";
            right_Parenthesis_number = 0;
            Is_Equalsign_Clicked = false;
            Memory_flag = false;
            Help_clicked = false;
        }

        private void button_multiSqrt_Click(object sender, EventArgs e)
        {
                this.theProgress.Text += "x√";
                expression += " x√ ";

        }
        private void button_plus_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "+";
            expression += " + ";

        }
        private void button_power_Click(object sender, EventArgs e)
        {
            this.theProgress.Text += "^";
            expression += " ^ ";

        }


        private void button_minus_Click(object sender, EventArgs e)
        {
            this.theProgress.Text += "-";
            expression += " - ";

        }


        private void button_multiply_Click(object sender, EventArgs e)
        {
            this.theProgress.Text += "*";
            expression += " * ";

        }

        private void button_division_Click(object sender, EventArgs e)
        {
            this.theProgress.Text += "/";
            expression += " / ";

        }
     
        public int right_Parenthesis_number = 0;//记录一共差几个右括号
        private void button_right_click(object sender, EventArgs e)
        {
            this.theProgress.Text += "(";
            expression += " ( ";
            right_Parenthesis_number++;
        }

        private void button_right_Click_1(object sender, EventArgs e)
        {
            if (right_Parenthesis_number>0)
            {
                this.theProgress.Text += ")";
                expression += " ) ";
               
                right_Parenthesis_number--;
            }
            

        }

        public bool Is_Equalsign_Clicked = false;//判断=是否已经按下
        private void button_result_Click(object sender, EventArgs e)
        {
            if (!Is_Equalsign_Clicked&&right_Parenthesis_number ==0){
                this.theProgress.Text += "=";
                string postfix = infixToPostfix(expression);
                double result = getResultUsePostfix(postfix);
                this.theResult.Text = Convert.ToString(result);
                this.History.Text += this.theProgress.Text + "\r\n" + this.theResult.Text + "\r\n"//历史记录的实现
                    + "———————————————";
                Is_Equalsign_Clicked = true;
            }
            
        }

        private void button_point_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (IsNumeric(current))
            {
                this.theProgress.Text += ".";
                expression += ".";
            }

        }


        private void button_minusBeforeNumber_Click(object sender, EventArgs e)
        {
            this.theProgress.Text += "-";
            expression += "-";
        }

        //特殊运算符的处理，为了保证所有操作都是 操作数a 操作符* 操作数b 这样的结构，
        //所以对于tan 这样无左值的运算都加了 补位左值1
        private void button_tan_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "tan";
                expression += " 1 tan ";
            }

        }
        private void button_tanh_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "tanh";
                expression += " 1 tanh ";
            }
        }

        private void button_cos_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "cos";
                expression += " 1 cos ";
            }

        }

        private void button_cosh_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "cosh";
                expression += " 1 cosh ";
            }
        }

        private void button_sin_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "sin";
                expression += " 1 sin ";
            }

        }

        private void button_sinh_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "sinh";
                expression += " 1 sinh ";
            }
        }

        private void button_sqrt_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "√";
                expression += " 1 √ ";
            }
        }
        private void button_log_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "lg";
                expression += " 1 lg ";
            }
        }
        private void button_ln_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "ln";
                expression += " 1 ln ";
            }
        }
        //阶乘，特殊处理
        private void button_factorial_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (IsNumeric(current))
            {
                this.theProgress.Text += "!";
                expression += " ! 1 ";
            }
        }

        private void button_ClearHistory_Click(object sender, EventArgs e)
        {
            this.History.Text = "";
        }

        public string MemorySave = "";//储存数据的盒子
        private void button_MS_Click(object sender, EventArgs e)
        {
            if (this.theResult.Text.Length > 0)
            {
                MemorySave = this.theResult.Text;
            }
        }

        public bool Memory_flag = false;//判断是否读取过当前memory中的数字
        private void button_MR_Click(object sender, EventArgs e)
        {
            if (MemorySave.Length > 0&&!Memory_flag)
            {
                this.History.Text+="\r\n"+"Memory:"+ MemorySave+"\r\n";
                expression += " " + MemorySave;
                this.theProgress.Text += MemorySave;
                Memory_flag = true;
            }
            
        }

        private void button_MC_Click(object sender, EventArgs e)
        {
            MemorySave = "";
            Memory_flag = false;
        }

        private void button_MPlus_Click(object sender, EventArgs e)
        {
            if (MemorySave.Length > 0 && this.theResult.Text.Length > 0)
            {
                double result = Convert.ToDouble(this.theResult.Text);
                double memory = Convert.ToDouble(MemorySave);
                memory += result;
                MemorySave = Convert.ToString(memory);
                Memory_flag = false;
            }
        }

        private void button_Mminus_Click(object sender, EventArgs e)
        {
            if (MemorySave.Length > 0 && this.theResult.Text.Length > 0)
            {
                double result = Convert.ToDouble(this.theResult.Text);
                double memory = Convert.ToDouble(MemorySave);
                memory -= result;
                MemorySave = Convert.ToString(memory);
                Memory_flag = false;
            }
        }
        public bool Help_clicked = false;//判断是否help按键已按下
        private void button_help_Click(object sender, EventArgs e)
        {
            if (!Help_clicked)
            {
                Help helpform = new Help();
                helpform.Show();
                Help_clicked = true;
            }
           
        }

        private void button_pi_Click(object sender, EventArgs e)
        {//pi前不能直接 接数字
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "π";
                expression += Convert.ToDouble(Math.PI);
            }

        }

        private void button_e_Click(object sender, EventArgs e)
        {
            string current = Current(expression);
            if (!IsNumeric(current))
            {
                this.theProgress.Text += "e";
                expression += Convert.ToDouble(Math.E);
            }

        }

       
    }
}
