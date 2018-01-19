// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using System.Threading.Tasks;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace FBClone.Model
{
    public partial class SpGetAnswersByQuestionIdReturnModel
    {
        public String Id { get; set; }
        public String QuestionId { get; set; }
        public String AnswerText { get; set; }
        public Double AnswerFactor { get; set; }
        public Int32 Sequence { get; set; }
        public String Userid { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public String UpdatedBy { get; set; }
        public Byte[] Version { get; set; }
        public Boolean Deleted { get; set; }
    }

}
