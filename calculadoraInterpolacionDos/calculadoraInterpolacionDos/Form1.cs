using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunctionParser;

namespace calculadoraInterpolacionDos
{
    public partial class Form1 : Form
    {
       
        string[] idsNames = new string[] { };
        double[] idsValues = new double[] { };

        int  x0, x1, contador = 0;
        double x,fx, fx0, fx1, et, vr;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Asignar();
            Tabla(x0, x, x1);
            txt_expr.Focus();
            lblValorReal.Text = Math.Log(x).ToString();
        }

        private void CreateTreeNodes(ParsTreeNode parseNode, TreeNode treeViewNode)
        {
            if (parseNode is Expression)
            {
                Expression expr = parseNode as Expression;
                switch (expr.Expansion)
                {
                    case Expression.ExpressionExpansion.ExpressionMinusTerm:
                        TreeNode subExpr = new TreeNode("Expression: " + expr.SubExpression.Value);
                        subExpr.Tag = expr.SubExpression;
                        CreateTreeNodes(expr.SubExpression, subExpr);
                        treeViewNode.Nodes.Add(subExpr);
                        treeViewNode.Nodes.Add("-");
                        TreeNode termNode = new TreeNode("Term: " + expr.Term.Value);
                        termNode.Tag = expr.Term;
                        CreateTreeNodes(expr.Term, termNode);
                        treeViewNode.Nodes.Add(termNode);
                        break;
                    case Expression.ExpressionExpansion.ExpressionPlusTerm:
                        TreeNode subExpr2 = new TreeNode("Expression: " + expr.SubExpression.Value);
                        subExpr2.Tag = expr.SubExpression;
                        CreateTreeNodes(expr.SubExpression, subExpr2);
                        TreeNode termNode2 = new TreeNode("Term: " + expr.Term.Value);
                        termNode2.Tag = expr.Term;
                        CreateTreeNodes(expr.Term, termNode2);

                        treeViewNode.Nodes.Add(subExpr2);
                        treeViewNode.Nodes.Add("+");
                        treeViewNode.Nodes.Add(termNode2);

                        break;
                    case Expression.ExpressionExpansion.Term:
                        TreeNode termNode3 = new TreeNode("Term: " + expr.Value);
                        termNode3.Tag = expr;
                        CreateTreeNodes(expr.Term, termNode3);
                        treeViewNode.Nodes.Add(termNode3);
                        break;
                }
            }
            else if (parseNode is Term)
            {
                Term term = parseNode as Term;
                switch (term.Expansion)
                {
                    case Term.TermExpansion.TermMulFactor:
                        TreeNode subTerm = new TreeNode("Term: " + term.SubTerm.Value);
                        subTerm.Tag = term.SubTerm;
                        CreateTreeNodes(term.SubTerm, subTerm);                        TreeNode factor = new TreeNode("Factor: " + term.Factor.Value);
                        factor.Tag = term.Factor;
                        CreateTreeNodes(term.Factor, factor);

                        treeViewNode.Nodes.Add(subTerm);
                        treeViewNode.Nodes.Add("*");
                        treeViewNode.Nodes.Add(factor);

                        break;

                    case Term.TermExpansion.TermDivFactor:
                        TreeNode subTerm2 = new TreeNode("Term: " + term.SubTerm.Value);
                        subTerm2.Tag = term.SubTerm;
                        CreateTreeNodes(term.SubTerm, subTerm2);
                        TreeNode factor2 = new TreeNode("Factor: " + term.Factor.Value);
                        factor2.Tag = term.Factor;
                        CreateTreeNodes(term.Factor, factor2);

                        treeViewNode.Nodes.Add(subTerm2);
                        treeViewNode.Nodes.Add("/");
                        treeViewNode.Nodes.Add(factor2);
                        break;

                    case Term.TermExpansion.TermPowFactor:
                        TreeNode subTerm3 = new TreeNode("Term: " + term.SubTerm.Value);
                        subTerm3.Tag = term.SubTerm;
                        CreateTreeNodes(term.SubTerm, subTerm3);
                        TreeNode factor3 = new TreeNode("Factor: " + term.Factor.Value);
                        factor3.Tag = term.Factor;
                        CreateTreeNodes(term.Factor, factor3);

                        treeViewNode.Nodes.Add(subTerm3);
                        treeViewNode.Nodes.Add("^");
                        treeViewNode.Nodes.Add(factor3);
                        break;
                    case Term.TermExpansion.Factor:
                        TreeNode factor4 = new TreeNode("Factor: " + term.Factor.Value);
                        factor4.Tag = term.Factor;
                        CreateTreeNodes(term.Factor, factor4);
                        treeViewNode.Nodes.Add(factor4);
                        break;
                }
            }
            else if (parseNode is Factor)
            {
                Factor factor = parseNode as Factor;
                switch (factor.Expansion)
                {
                    case Factor.FactorExpansion.Number:
                        TreeNode number = new TreeNode("Number: " + factor.Value);
                        treeViewNode.Nodes.Add(number);
                        break;
                    case Factor.FactorExpansion.ID:
                        TreeNode id = new TreeNode("ID: " + factor.Value);
                        treeViewNode.Nodes.Add(id);
                        break;
                    case Factor.FactorExpansion.WrappedExpression:
                        treeViewNode.Nodes.Add("(");
                        TreeNode expr = new TreeNode("Expression:" + factor.WrappedExpression.Value);
                        expr.Tag = factor.WrappedExpression;
                        CreateTreeNodes(factor.WrappedExpression, expr);
                        treeViewNode.Nodes.Add(expr);
                        treeViewNode.Nodes.Add(")");
                        break;
                    case Factor.FactorExpansion.Function:
                        treeViewNode.Nodes.Add("Function: " + factor.Function.Func.ToString());
                        TreeNode term = new TreeNode("Term: " + factor.Function.Term.Value);
                        term.Tag = factor.Function.Term;
                        CreateTreeNodes(factor.Function.Term, term);
                        treeViewNode.Nodes.Add(term);
                        break;
                }
            }
        }
 
        public void txt_expr_TextChanged(object sender, EventArgs e)
        {

            if (Expression.IsExpression(txt_expr.Text, idsNames))
            {
                txt_expr.ForeColor = Color.Black;
                Expression expression = new Expression(txt_expr.Text, idsNames, null);
                TreeNode expr = new TreeNode(txt_expr.Text);
                CreateTreeNodes(expression, expr);
                double result = expression.CalculateValue(idsValues);
                lbl_result.Text = string.Format("{0:f3}", result);
            }
            else
            {
                txt_expr.ForeColor = Color.Red;
            }
        }
        private void lst_ids_KeyDown(object sender, KeyEventArgs e)
        {
            //delete character 
            if (e.KeyCode == Keys.Delete)
            {
                if (lst_ids.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lst_ids.SelectedItems.Count; i++)
                        lst_ids.Items.Remove(lst_ids.SelectedItems[i]);
                }
            }
            UpdateIDs();

        }

        private void btn_addID_Click(object sender, EventArgs e)
        {
            Form_ID frmID = new Form_ID();
            if (frmID.ShowDialog() == DialogResult.OK)
            {
                lst_ids.Items.Add(frmID.ID_Name).SubItems.Add(frmID.ID_Value);
            }
            UpdateIDs();
        }

        public void UpdateIDs()
        {
            idsNames = new string[lst_ids.Items.Count];
            idsValues = new double[lst_ids.Items.Count];
            for (int i = 0; i < lst_ids.Items.Count; i++)
            {
                idsNames[i] = lst_ids.Items[i].Text;
                idsValues[i] = double.Parse(lst_ids.Items[i].SubItems[1].Text);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        public void Asignar()
        {
            try
            {
                x = Convert.ToDouble(lbl_result.Text);
                x0 = Convert.ToInt32(txtX0.Text);
                x1 = Convert.ToInt32(txtX1.Text);
                vr = Math.Log(x);
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        public void Tabla(int x0, double x, int x1)
        {
            while (x > x0 || x < x1)
            {
                if (x == x0)
                    x0--;
                if (x == x1)
                    x1++;

                fx0 = Math.Log(x0);
                fx1 = Math.Log(x1);
                fx = fx0 + ((fx1 - fx0) / (x1 - x0)) * (x - x0);
                et = Math.Abs(vr - fx) * 100;
                dt.Rows.Add(contador, x0, x, x1, fx0, fx, fx1, et);
                if (x > x0) { x0++; }
                else { x0 = x0; }
                if (x < x1) { x1--; }
                else { x1 = x1; }
                contador++;
            }
        }
    }
}
