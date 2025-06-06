﻿/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.XSSF.UserModel.Charts;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace TestCases.XSSF.UserModel
{
    [TestFixture]
    internal class TestXSSFDateAxis
    {
        [Test]
        public void TestAccessMethods()
        {
            XSSFWorkbook wb = new XSSFWorkbook();
            XSSFSheet sheet = wb.CreateSheet() as XSSFSheet;
            XSSFDrawing Drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            XSSFClientAnchor anchor = Drawing.CreateAnchor(0, 0, 0, 0, 1, 1, 10, 30) as XSSFClientAnchor;
            XSSFChart chart = Drawing.CreateChart(anchor) as XSSFChart;
            XSSFDateAxis axis = chart.ChartAxisFactory.CreateDateAxis(AxisPosition.Bottom) as XSSFDateAxis;

            axis.Crosses = AxisCrosses.AutoZero;
            ClassicAssert.AreEqual(axis.Crosses, AxisCrosses.AutoZero);

            ClassicAssert.AreEqual(chart.GetAxis().Count, 1);
        }
    }
}