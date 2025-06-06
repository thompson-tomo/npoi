﻿/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
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

using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Linq;

namespace TestCases.XSSF.UserModel
{
    [TestFixture]
    public class TestXSSFSheetCopyTo
    {
        [Test]
        public void CopySheetToWorkbookShouldCopyFormulasOver()
        {
            XSSFWorkbook srcWorkbook = new XSSFWorkbook();
            XSSFSheet srcSheet = srcWorkbook.CreateSheet("Sheet1") as XSSFSheet;

            // Set some values
            IRow row1 = srcSheet.CreateRow((short)0);
            ICell cell = row1.CreateCell((short)0);
            cell.SetCellValue(1);
            ICell cell2 = row1.CreateCell((short)1);
            cell2.SetCellFormula("A1+1");
            XSSFWorkbook destWorkbook = new XSSFWorkbook();
            srcSheet.CopyTo(destWorkbook, srcSheet.SheetName, true, true);

            var destSheet = destWorkbook.GetSheet("Sheet1");
            ClassicAssert.NotNull(destSheet);

            ClassicAssert.AreEqual(1, destSheet.GetRow(0)?.GetCell(0).NumericCellValue);
            ClassicAssert.AreEqual("A1+1", destSheet.GetRow(0)?.GetCell(1).CellFormula);

            destSheet.GetRow(0)?.GetCell(0).SetCellValue(10);
            var evaluator = destWorkbook.GetCreationHelper()
                                        .CreateFormulaEvaluator();

            var destCell = destSheet.GetRow(0)?.GetCell(1);
            evaluator.EvaluateFormulaCell(destCell);
            var destCellValue = evaluator.Evaluate(destCell);

            ClassicAssert.AreEqual(11, destCellValue.NumberValue);
        }

        [Test]
        public void CopySheetToWorkbookShouldCopyMergedRegionsOver()
        {
            XSSFWorkbook srcWorkbook = new XSSFWorkbook();
            XSSFSheet srcSheet = srcWorkbook.CreateSheet("Sheet1") as XSSFSheet;

            // Set some merged regions 
            srcSheet.AddMergedRegion(CellRangeAddress.ValueOf("A1:B4"));
            srcSheet.AddMergedRegion(CellRangeAddress.ValueOf("C1:F40"));


            XSSFWorkbook destWorkbook = new XSSFWorkbook();
            srcSheet.CopyTo(destWorkbook, srcSheet.SheetName, true, true);

            var destSheet = destWorkbook.GetSheet("Sheet1");
            ClassicAssert.NotNull(destSheet);
            ClassicAssert.AreEqual(2, destSheet.MergedRegions.Count);

            ClassicAssert.IsTrue(
                new string[]
                {
                    "A1:B4",
                    "C1:F40"
                }
                .SequenceEqual(
                    destSheet.MergedRegions
                             .Select(r => r.FormatAsString())));
        }

        [Test]
        public void CopySheetToWorkbookShouldCopyCharts()
        {
            XSSFWorkbook sourceWorkbook = XSSFTestDataSamples.OpenSampleWorkbook("WithThreeCharts.xlsx");

            XSSFSheet srcDataSheet = (XSSFSheet)sourceWorkbook.GetSheetAt(0);
            XSSFWorkbook destWorkbook = new XSSFWorkbook();
            srcDataSheet.CopyTo(destWorkbook, srcDataSheet.SheetName, true, true);

            XSSFSheet srcChartSheet = (XSSFSheet)sourceWorkbook.GetSheetAt(1);
            srcChartSheet.CopyTo(destWorkbook, srcChartSheet.SheetName, true, true);

            var destSheet = destWorkbook.GetSheetAt(1);
            
            ClassicAssert.NotNull(destSheet);

            ClassicAssert.AreEqual(2, (srcChartSheet.CreateDrawingPatriarch() as XSSFDrawing).GetCharts().Count);
            ClassicAssert.AreEqual(2, (destSheet.CreateDrawingPatriarch() as XSSFDrawing).GetCharts().Count);
            ClassicAssert.AreEqual(2, (destSheet.CreateDrawingPatriarch() as XSSFDrawing).GetCharts()[0].GetAxis().Count);
        }
    }
}