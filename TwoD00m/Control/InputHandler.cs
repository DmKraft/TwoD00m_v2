using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TwoD00m.Control
{
    public class InputHandler
    {
        private List<Button> buttons = new List<Button>();
        
        public InputHandler()
        {
            buttons.Add(new Button().setCommand<Use>().setKey(Keys.F));
            buttons.Add(new Button().setCommand<StepFront>().setKey(Keys.W));
            buttons.Add(new Button().setCommand<StepBack>().setKey(Keys.S));
            buttons.Add(new Button().setCommand<StepLeft>().setKey(Keys.A));
            buttons.Add(new Button().setCommand<StepRight>().setKey(Keys.D));
            buttons.Add(new Button().setCommand<TurnLeft>().setKey(Keys.Q));
            buttons.Add(new Button().setCommand<TurnRight>().setKey(Keys.E));
            buttons.Add(new Button().setCommand<Attack>().setKey(Keys.Space));
            buttons.Add(new Button().setCommand<UsePotion>().setKey(Keys.D3));
            buttons.Add(new Button().setCommand<PlayerHUD>().setKey(Keys.LeftShift));

            /*buttons.Add(new Button().setCommand<StepFront>().setKey(Keys.Up));
            buttons.Add(new Button().setCommand<StepBack>().setKey(Keys.Back));
            buttons.Add(new Button().setCommand<StepLeft>().setKey(Keys.Left));
            buttons.Add(new Button().setCommand<StepRight>().setKey(Keys.Right));
            buttons.Add(new Button().setCommand<TurnLeft>().setKey(Keys.PageUp));
            buttons.Add(new Button().setCommand<TurnRight>().setKey(Keys.PageDown));*/

        }

        public InputHandler(int inventoryIsOpen)
        {
            buttons.Add(new Button().setCommand<SetPotion>().setKey(Keys.D3));
            buttons.Add(new Button().setCommand<SetLeftHandWeapon>().setKey(Keys.D1));
            buttons.Add(new Button().setCommand<SetRightHandWeapon>().setKey(Keys.D2));
            buttons.Add(new Button().setCommand<GetLeftElement>().setKey(Keys.A));
            buttons.Add(new Button().setCommand<GetRightElement>().setKey(Keys.D));
        }

        public Command handleInput()
        {
            Command command = new Command() ;
            buttons.ForEach(delegate (Button button)
            {
                if (button.processPresing())
                    command = button.getCommand();
            });
            
            if (command.GetType().IsSubclassOf(typeof(Command)))
                return command;
            else
                return null;
        }

        public class Button
        {
            private Keys key;
            private Command command;

            public Button() { }

            public Button setKey(Keys key)
            {
                this.key = key;
                return this;
            }

            public Button setCommand<TCommand>() where TCommand : Command, new()
            {
                command = new TCommand();
                return this;
            }

            public Command getCommand() { return command; }

            private Keys[] keysOld = Keyboard.GetState().GetPressedKeys();

            public bool processPresing()
            {
                Keys[] keyNew = Keyboard.GetState().GetPressedKeys();
                if (keyNew.Length == 1)
                {
                    if (keyNew.Contains(key) && !keysOld.Contains(key))
                    {
                        keysOld = keyNew;
                        return true;
                    }
                }
                if (keysOld.Length != 0)
                    if (Keyboard.GetState().IsKeyUp(keysOld[0]))
                        keysOld = keyNew;
                return false;
            }
        }
    }
}