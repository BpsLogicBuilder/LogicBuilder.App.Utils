using LogicBuilder.Attributes;
using LogicBuilder.Domain;


namespace Contoso.Domain.Entities
{
    public class RulesModuleModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("RulesModule_RulesModuleId")]
		public int RulesModuleId { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("RulesModule_Name")]
		public string Name { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("RulesModule_Application")]
		public string Application { get; set; }

		[ListEditorControl(ListControlType.HashSetForm)]
		[AlsoKnownAs("RulesModule_RuleSetFile")]
		public byte[] RuleSetFile { get; set; }

		[ListEditorControl(ListControlType.HashSetForm)]
		[AlsoKnownAs("RulesModule_ResourceSetFile")]
		public byte[] ResourceSetFile { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("RulesModule_LoggedInUserId")]
		public string LoggedInUserId { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("RulesModule_LastUpdated")]
		public System.DateTime LastUpdated { get; set; }
    }
}