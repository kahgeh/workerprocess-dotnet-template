using System.Runtime.Serialization;

namespace WorkerProcess.Template.Contracts
{
    public enum JobStatus
    {
        [EnumMember(Value = "INPROGRESS")]
        InProgress,
        [EnumMember(Value = "SUCCESS")]
        Successful,
        [EnumMember(Value = "FAILURE")]
        Failure,
    }
}