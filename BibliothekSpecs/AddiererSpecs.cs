using Bibliothek;
using FluentAssertions;
using NUnit.Framework;

namespace BibliothekSpecs
{
    [TestFixture]
    public class AddiererSpecs
    {
        [Test]
        public void Addiert_zwei_Zahlen()
        {
            var subject = new Addierer();
            subject.Addiere(22, 20).Should().Be(42);
        }
    }
}
