namespace FizzBuzzCalc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FlaUI.Core.AutomationElements;

    internal class Calculator
    {
        private const string ClearEntryButtonId = "clearEntryButton";
        private const string ClearButtonId = "clearButton";
        private const string ClearHistoryButtonId = "ClearHistory";

        private const string DivideButtonId = "divideButton";
        private const string EqualsButtonId = "equalButton";
        private const string ResultsId = "CalculatorResults";

        private readonly Window window;
        private readonly Dictionary<byte, AutomationElement> numberButtons = new Dictionary<byte, AutomationElement>();

        private readonly Button divideButton;
        private readonly Button equalsButton;
        private readonly AutomationElement resultsElement;

        public decimal Result
        {
            get
            {
                var resultsElementName = resultsElement.Name;
                var resultsString = resultsElementName.Substring(resultsElementName.LastIndexOf(' ')) + 1;

                if (!decimal.TryParse(resultsString, NumberStyles.Number, CultureInfo.CurrentCulture, out var result))
                {
                    throw new InvalidOperationException($"Value from results box was not a number, got: {resultsString}.");
                }

                return result;
            }
        }

        public Calculator(Window window)
        {
            this.window = window ?? throw new ArgumentNullException(nameof(window));

            divideButton = GetButton(window, DivideButtonId);
            equalsButton = GetButton(window, EqualsButtonId);
            resultsElement = this.window.FindFirstDescendant(x => x.ByAutomationId(ResultsId))
                           ?? throw new InvalidOperationException($"Could not find results element with ID: {ResultsId}.");
        }

        public void EnterNumber(int value)
        {
            var passes = (int)Math.Log10(value);

            for (var i = passes; i >= 0; i--)
            {
                var divisor = (int)Math.Pow(10, i);
                var divided = value / divisor;
                var digit = divided % 10;
                PressNumberButton((byte)digit);
            }
        }

        private void PressNumberButton(byte value)
        {
            if (value > 9)
            {
                throw new ArgumentOutOfRangeException($"Value cannot be greater than 9. Got: {value}.");
            }

            if (!numberButtons.TryGetValue(value, out var button))
            {
                button = GetButton(window, $"num{value}Button");
                numberButtons[value] = button;
            }

            button.Click();
        }

        public void PressDivide()
        {
            divideButton.Click();
        }

        public void PressEquals()
        {
            equalsButton.Click();
        }

        public void PressClear()
        {
            Button clearable;

            var clearEntryButton = GetButton(window, ClearEntryButtonId, false);

            if (clearEntryButton == null)
            {
                var clearButton = GetButton(window, ClearButtonId);
                
                clearable = clearButton;
            }
            else
            {
                clearable = clearEntryButton;
            }

            clearable.Click();
        }

        public decimal Divide(int dividend, int divisor)
        {
            PressClear();
            EnterNumber(dividend);
            PressDivide();
            EnterNumber(divisor);
            PressEquals();

            return Result;
        }

        public void ClearHistory()
        {
            var button = GetButton(window, ClearHistoryButtonId, false);

            button?.Click();
        }

        private static Button GetButton(Window window, string id, bool shouldThrow = true)
        {
            var button = window.FindFirstDescendant(x => x.ByAutomationId(id))?.AsButton();

            if (button == null && shouldThrow)
            {
                throw new InvalidOperationException($"Could not find button with ID: {id}.");
            }

            return button;
        }
    }
}