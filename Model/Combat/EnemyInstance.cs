using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Utilities.EntriesValidation;
using ProjetPOO.Utilities.Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class EnemyInstance : Character
    {
        private int _templateId;
        private EnemyType _type;

        public int TemplateId
        {
            get => _templateId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _templateId = value;
                }
            }
        }

        public EnemyType Type
        {
            get => _type;
            private set => _type = value;
        }

        public EnemyInstance(Enemy template)
            : base(template.Name, template.MaxHp, template.Attack, template.Defense, template.Agility)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            TemplateId = template.Id;
            Type = template.Type;
        }

    }
}
