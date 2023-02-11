using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TestClass;
using System;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void 足し算ルーチンのテスト()
    {
        //CalcAddはStage_Scriptに
        CalcAdd calcAdd = new CalcAdd();
        int actual = calcAdd.AddInt(1, 2);

        // 実際の戻り値 actual と　期待する結果 3 とを比較し正しいか検証
        Assert.That(actual, Is.EqualTo(3));
    }
    [Test]
    public void zeroを消すテスト()
    {
        BringBeforeZero bringBeforeZero = new BringBeforeZero();
        int[] input = { 0, 1, 2, 0, 4 };
        int[] actual = bringBeforeZero.Main(input);
        int[] correct = { 1, 2, 4 };
        Assert.That(actual, Is.EqualTo(correct));
    }
    [Test]
    public void zeroを前に持ってくるテスト()
    {
        BringBeforeZero bringBeforeZero = new BringBeforeZero();
        int[] input = { 1, 2, 4 };
        int count_0 = 2;
        int[] actual = bringBeforeZero.Add0ToHead(input,count_0);
        int[] correct = { 0, 0 , 1, 2, 4 };
        Assert.That(actual, Is.EqualTo(correct));
    }
    [Test]
    public void zeroを消して前に持ってくるテスト()
    {
        BringBeforeZero bringBeforeZero = new BringBeforeZero();
        int[] input = { 0, 1, 2, 0, 4, 0};
        int count_0 = 3;
        int[] actual = bringBeforeZero.BringBeforeZeroFunc(input, count_0);
        int[] correct = { 0, 0, 0, 1, 2, 4 };
        Assert.That(actual, Is.EqualTo(correct));
    }
}
