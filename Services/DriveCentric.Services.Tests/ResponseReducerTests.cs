using DriveCentric.BaseService;
using DriveCentric.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DriveCentric.Services.Tests
{
    public class ResponseReducerTests
    {
        [TestMethod]
        public void ResponseReducer_IsSimple_ReturnsTrue()
        {
            var reducer = new ResponseReducer();
            Assert.IsTrue(reducer.IsSimple(typeof(int)));
        }

        [TestMethod]
        public void ResponseReducer_IsSimple_ReturnsFalse()
        {
            var reducer = new ResponseReducer();
            Assert.IsTrue(reducer.IsSimple(typeof(UserTask)));
        }
    }
}