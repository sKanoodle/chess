﻿using Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestClass]
    public class WholeGame
    {
        [DataTestMethod]
        [DataRow("e4.d5.exd5.Qxd5.Nc3.Qa5.d4.Nf6.Nf3.c6.Bc4.Bf5.Ne5.e6.g4.Bg6.h4.Nbd7.Nxd7.Nxd7.h5.Be4.Rh3.Bg2.Re3.Nb6.Bd3.Nd5.f3.Bb4.Kf2.Bxc3.bxc3.Qxc3.Rb1.Qxd4.Rxb7.Rd8.h6.gxh6.Bg6.Ne7.Qxd4.Rxd4.Rd3.Rd8.Rxd8+.Kxd8.Bd3.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1018785")]
        [DataRow("e4.c5.Nf3.Nc6.d4.cxd4.Nxd4.Nf6.Nc3.d6.Bg5.e6.Qd2.a6.O-O-O.h6.Be3.Be7.f4.Nxd4.Bxd4.b5.Qe3.Qc7.e5.dxe5.Bxe5.Ng4.Qf3.Nxe5.Qxa8.Nd7.g3.Nb6.Qf3.Bb7.Ne4.f5.Qh5+.Kf8.Nf2.Bf6.Bd3.Na4.Rhe1.Bxb2+.Kb1.Bd5.Bxb5.Bxa2+.Kxa2.axb5.Kb1.Qa5.Nd3.Ba3.Ka2.Nc3+.Kb3.Nd5.Ka2.Bb4+.Kb1.Bc3.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1060694")]
        [DataRow("e4.d6.d4.Nf6.Nc3.g6.Be3.Bg7.Qd2.c6.f3.b5.Nge2.Nbd7.Bh6.Bxh6.Qxh6.Bb7.a3.e5.O-O-O.Qe7.Kb1.a6.Nc1.O-O-O.Nb3.exd4.Rxd4.c5.Rd1.Nb6.g3.Kb8.Na5.Ba8.Bh3.d5.Qf4+.Ka7.Rhe1.d4.Nd5.Nbxd5.exd5.Qd6.Rxd4.cxd4.Re7+.Kb6.Qxd4+.Kxa5.b4+.Ka4.Qc3.Qxd5.Ra7.Bb7.Rxb7.Qc4.Qxf6.Kxa3.Qxa6+.Kxb4.c3+.Kxc3.Qa1+.Kd2.Qb2+.Kd1.Bf1.Rd2.Rd7.Rxd7.Bxc4.bxc4.Qxh8.Rd3.Qa8.c3.Qa4+.Ke1.f4.f5.Kc1.Rd2.Qa7.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1011478")]
        [DataRow("d4.d5.c4.e6.Nf3.c5.cxd5.exd5.g3.Nf6.Bg2.Be7.O-O.O-O.Nc3.Nc6.Bg5.cxd4.Nxd4.h6.Be3.Re8.Qb3.Na5.Qc2.Bg4.Nf5.Rc8.Bd4.Bc5.Bxc5.Rxc5.Ne3.Be6.Rad1.Qc8.Qa4.Rd8.Rd3.a6.Rfd1.Nc4.Nxc4.Rxc4.Qa5.Rc5.Qb6.Rd7.Rd4.Qc7.Qxc7.Rdxc7.h3.h5.a3.g6.e3.Kg7.Kh2.Rc4.Bf3.b5.Kg2.R7c5.Rxc4.Rxc4.Rd4.Kf8.Be2.Rxd4.exd4.Ke7.Na2.Bc8.Nb4.Kd6.f3.Ng8.h4.Nh6.Kf2.Nf5.Nc2.f6.Bd3.g5.Bxf5.Bxf5.Ne3.Bb1.b4.gxh4.Ng2.hxg3+.Kxg3.Ke6.Nf4+.Kf5.Nxh5.Ke6.Nf4+.Kd6.Kg4.Bc2.Kh5.Bd1.Kg6.Ke7.Nxd5+.Ke6.Nc7+.Kd7.Nxa6.Bxf3.Kxf6.Kd6.Kf5.Kd5.Kf4.Bh1.Ke3.Kc4.Nc5.Bc6.Nd3.Bg2.Ne5+.Kc3.Ng6.Kc4.Ne7.Bb7.Nf5.Bg2.Nd6+.Kb3.Nxb5.Ka4.Nd6.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1067121")]
        [DataRow("Nf3.Nf6.c4.g6.Nc3.Bg7.d4.O-O.Bf4.d5.Qb3.dxc4.Qxc4.c6.e4.Nbd7.Rd1.Nb6.Qc5.Bg4.Bg5.Na4.Qa3.Nxc3.bxc3.Nxe4.Bxe7.Qb6.Bc4.Nxc3.Bc5.Rfe8+.Kf1.Be6.Bxb6.Bxc4+.Kg1.Ne2+.Kf1.Nxd4+.Kg1.Ne2+.Kf1.Nc3+.Kg1.axb6.Qb4.Ra4.Qxb6.Nxd1.h3.Rxa2.Kh2.Nxf2.Re1.Rxe1.Qd8+.Bf8.Nxe1.Bd5.Nf3.Ne4.Qb8.b5.h4.h5.Ne5.Kg7.Kg1.Bc5+.Kf1.Ng3+.Ke1.Bb4+.Kd1.Bb3+.Kc1.Ne2+.Kb1.Nc3+.Kc1.Rc2#.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1008361")]
        [DataRow("e4.e5.f4.exf4.Nf3.d5.exd5.Bd6.Nc3.Ne7.d4.O-O.Bd3.Nd7.O-O.h6.Ne4.Nxd5.c4.Ne3.Bxe3.fxe3.c5.Be7.Bc2.Re8.Qd3.e2.Nd6.Nf8.Nxf7.exf1=Q+.Rxf1.Bf5.Qxf5.Qd7.Qf4.Bf6.N3e5.Qe7.Bb3.Bxe5.Nxe5+.Kh7.Qe4+.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1034110")]
        [DataRow("Nf3.Nf6.g3.g6.c4.Bg7.Bg2.O-O.O-O.Nc6.Nc3.d6.d4.a6.d5.Na5.Nd2.c5.Qc2.e5.b3.Ng4.e4.f5.exf5.gxf5.Nd1.b5.f3.e4.Bb2.exf3.Bxf3.Bxb2.Qxb2.Ne5.Be2.f4.gxf4.Bh3.Ne3.Bxf1.Rxf1.Ng6.Bg4.Nxf4.Rxf4.Rxf4.Be6+.Rf7.Ne4.Qh4.Nxd6.Qg5+.Kh1.Ra7.Bxf7+.Rxf7.Qh8+.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1106725")]
        [DataRow("e4.c5.Nf3.Nc6.d4.cxd4.Nxd4.e6.Nc3.d6.Be3.Nf6.f4.Be7.Qf3.O-O.O-O-O.Qc7.Ndb5.Qb8.g4.a6.Nd4.Nxd4.Bxd4.b5.g5.Nd7.Bd3.b4.Nd5.exd5.exd5.f5.Rde1.Rf7.h4.Bb7.Bxf5.Rxf5.Rxe7.Ne5.Qe4.Qf8.fxe5.Rf4.Qe3.Rf3.Qe2.Qxe7.Qxf3.dxe5.Re1.Rd8.Rxe5.Qd6.Qf4.Rf8.Qe4.b3.axb3.Rf1+.Kd2.Qb4+.c3.Qd6.Bc5.Qxc5.Re8+.Rf8.Qe6+.Kh8.Qf7.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1139729")]
        [DataRow("d4.Nf6.c4.g6.g3.Bg7.Bg2.O-O.Nc3.d6.Nf3.Nbd7.O-O.e5.e4.c6.Be3.Ng4.Bg5.Qb6.h3.exd4.Na4.Qa6.hxg4.b5.Nxd4.bxa4.Nxc6.Qxc6.e5.Qxc4.Bxa8.Nxe5.Rc1.Qb4.a3.Qxb2.Qxa4.Bb7.Rb1.Nf3+.Kh1.Bxa8.Rxb2.Nxg5+.Kh2.Nf3+.Kh3.Bxb2.Qxa7.Be4.a4.Kg7.Rd1.Be5.Qe7.Rc8.a5.Rc2.Kg2.Nd4+.Kf1.Bf3.Rb1.Nc6.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1032320")]
        [DataRow("d4.Nf6.c4.e6.Nc3.Bb4.e3.d5.a3.Bxc3+.bxc3.c5.cxd5.exd5.Bd3.O-O.Ne2.b6.O-O.Ba6.Bxa6.Nxa6.Bb2.Qd7.a4.Rfe8.Qd3.c4.Qc2.Nb8.Rae1.Nc6.Ng3.Na5.f3.Nb3.e4.Qxa4.e5.Nd7.Qf2.g6.f4.f5.exf6.Nxf6.f5.Rxe1.Rxe1.Re8.Re6.Rxe6.fxe6.Kg7.Qf4.Qe8.Qe5.Qe7.Ba3.Qxa3.Nh5+.gxh5.Qg5+.Kf8.Qxf6+.Kg8.e7.Qc1+.Kf2.Qc2+.Kg3.Qd3+.Kh4.Qe4+.Kxh5.Qe2+.Kh4.Qe4+.g4.Qe1+.Kh5.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1031957")]
        [DataRow("e4.e5.Nf3.Nc6.Bc4.Bc5.c3.Bb6.d4.Qe7.O-O.d6.h3.Nf6.Re1.O-O.Na3.Nd8.Bf1.Ne8.Nc4.f6.a4.c6.Nxb6.axb6.Qb3+.Ne6.Qxb6.g5.Bc4.h6.h4.Kh7.hxg5.hxg5.dxe5.dxe5.Be3.Rh8.g3.Kg6.Kg2.Nf4+.gxf4.Bh3+.Kg3.exf4+.Bxf4.Qd7.Nh2.gxf4+.Kxf4.Rh4+.Ke3.Bg2.Nf3.Rxe4+.Kxe4.Nd6+.Kd3.Qf5+.Kd4.Qf4+.Kd3.Qxc4+.Kc2.Bxf3.b3.Be4+.Kb2.Qd3.Rg1+.Kf7.Rac1.Qd2+.Ka3.Nc4+.bxc4.Rxa4+.Kxa4.Qa2+.Kb4.Qb2+.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1156038")]
        [DataRow("d4.f5.c4.Nf6.g3.e6.Bg2.Bb4+.Bd2.Bxd2+.Nxd2.Nc6.Ngf3.O-O.O-O.d6.Qb3.Kh8.Qc3.e5.e3.a5.b3.Qe8.a3.Qh5.h4.Ng4.Ng5.Bd7.f3.Nf6.f4.e4.Rfd1.h6.Nh3.d5.Nf1.Ne7.a4.Nc6.Rd2.Nb4.Bh1.Qe8.Rg2.dxc4.bxc4.Bxa4.Nf2.Bd7.Nd2.b5.Nd1.Nd3.Rxa5.b4.Rxa8.bxc3.Rxe8.c2.Rxf8+.Kh7.Nf2.c1=Q+.Nf1.Ne1.Rh2.Qxc4.Rb8.Bb5.Rxb5.Qxb5.g4.Nf3+.Bxf3.exf3.gxf5.Qe2.d5.Kg8.h5.Kh7.e4.Nxe4.Nxe4.Qxe4.d6.cxd6.f6.gxf6.Rd2.Qe2.Rxe2.fxe2.Kf2.exf1=Q+.Kxf1.Kg7.Ke2.Kf7.Ke3.Ke6.Ke4.d5+.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1012099")]
        [DataRow("d4.e6.Nf3.f5.c4.Nf6.Bg5.Be7.Nc3.O-O.e3.b6.Bd3.Bb7.O-O.Qe8.Qe2.Ne4.Bxe7.Nxc3.bxc3.Qxe7.a4.Bxf3.Qxf3.Nc6.Rfb1.Rae8.Qh3.Rf6.f4.Na5.Qf3.d6.Re1.Qd7.e4.fxe4.Qxe4.g6.g3.Kf8.Kg2.Rf7.h4.d5.cxd5.exd5.Qxe8+.Qxe8.Rxe8+.Kxe8.h5.Rf6.hxg6.hxg6.Rh1.Kf8.Rh7.Rc6.g4.Nc4.g5.Ne3+.Kf3.Nf5.Bxf5.gxf5.Kg3.Rxc3+.Kh4.Rf3.g6.Rxf4+.Kg5.Re4.Kf6.Kg8.Rg7+.Kh8.Rxc7.Re8.Kxf5.Re4.Kf6.Rf4+.Ke5.Rg4.g7+.Kg8.Rxa7.Rg1.Kxd5.Rc1.Kd6.Rc2.d5.Rc1.Rc7.Ra1.Kc6.Rxa4.d6.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1102104")]
        [DataRow("f4.d5.e3.Nf6.b3.e6.Bb2.Be7.Bd3.b6.Nc3.Bb7.Nf3.Nbd7.O-O.O-O.Ne2.c5.Ng3.Qc7.Ne5.Nxe5.Bxe5.Qc6.Qe2.a6.Nh5.Nxh5.Bxh7+.Kxh7.Qxh5+.Kg8.Bxg7.Kxg7.Qg4+.Kh7.Rf3.e5.Rh3+.Qh6.Rxh6+.Kxh6.Qd7.Bf6.Qxb7.Kg7.Rf1.Rab8.Qd7.Rfd8.Qg4+.Kf8.fxe5.Bg7.e6.Rb7.Qg6.f6.Rxf6+.Bxf6.Qxf6+.Ke8.Qh8+.Ke7.Qg7+.Kxe6.Qxb7.Rd6.Qxa6.d4.exd4.cxd4.h4.d3.Qxd3.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1026352")]
        [DataRow("e4.e5.Nf3.Nc6.Bc4.Bc5.c3.Nf6.d4.exd4.cxd4.Bb4+.Nc3.d5.exd5.Nxd5.O-O.Be6.Bg5.Be7.Bxd5.Bxd5.Nxd5.Qxd5.Bxe7.Nxe7.Re1.f6.Qe2.Qd7.Rac1.c6.d5.cxd5.Nd4.Kf7.Ne6.Rhc8.Qg4.g6.Ng5+.Ke8.Rxe7+.Kf8.Rf7+.Kg8.Rg7+.Kh8.Rxh7+.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1132699")]
        [DataRow("e4.e5.Nf3.d6.d4.Bg4.dxe5.Bxf3.Qxf3.dxe5.Bc4.Nf6.Qb3.Qe7.Nc3.c6.Bg5.b5.Nxb5.cxb5.Bxb5+.Nbd7.O-O-O.Rd8.Rxd7.Rxd7.Rd1.Qe6.Bxd7+.Nxd7.Qb8+.Nxb8.Rd8#.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1233404")]
        [DataRow("e4.e5.f4.exf4.Bc4.Qh4+.Kf1.b5.Bxb5.Nf6.Nf3.Qh6.d3.Nh5.Nh4.Qg5.Nf5.c6.g4.Nf6.Rg1.cxb5.h4.Qg6.h5.Qg5.Qf3.Ng8.Bxf4.Qf6.Nc3.Bc5.Nd5.Qxb2.Bd6.Bxg1.e5.Qxa1+.Ke2.Na6.Nxg7+.Kd8.Qf6+.Nxf6.Be7#.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1018910")]
        [DataRow("e4.e5.Bc4.Bc5.d4.Bxd4.Nf3.Nc6.O-O.Nf6.Nxd4.Nxd4.f4.d6.fxe5.dxe5.Bg5.Be6.Bxe6.Nxe6.Qxd8+.Rxd8.Bxf6.gxf6.Rxf6.Nf4.Nc3.Rd2.Rd1.Rxg2+.Kh1.Rhg8.Rf5.f6.Rxf6.Nh3.Rff1.Rg1+.Rxg1.Nf2#.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1271822")]
        [DataRow("e4.c5.Nf3.Nc6.d4.cxd4.Nxd4.e5.Nxc6.bxc6.Bc4.Nf6.Bg5.Be7.Qe2.d5.Bxf6.Bxf6.Bb3.O-O.O-O.a5.exd5.cxd5.Rd1.d4.c4.Qb6.Bc2.Bb7.Nd2.Rae8.Ne4.Bd8.c5.Qc6.f3.Be7.Rac1.f5.Qc4+.Kh8.Ba4.Qh6.Bxe8.fxe4.c6.exf3.Rc2.Qe3+.Kh1.Bc8.Bd7.f2.Rf1.d3.Rc3.Bxd7.cxd7.e4.Qc8.Bd8.Qc4.Qe1.Rc1.d2.Qc5.Rg8.Rd1.e3.Qc3.Qxd1.Rxd1.e2.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1001165")]
        [DataRow("e4.e5.Nf3.Nc6.Bb5.a6.Ba4.Nf6.O-O.Be7.Re1.b5.Bb3.O-O.c3.d5.exd5.Nxd5.Nxe5.Nxe5.Rxe5.c6.d3.Bd6.Re1.Qh4.g3.Qh3.Re4.Qd7.Nd2.Bb7.Qf1.c5.Re1.Kh8.a4.Nf4.Ne4.Nh3+.Kh1.c4.dxc4.Nxf2+.Qxf2.f5.Qd4.fxe4.Be3.Qh3.Qxd6.Rf2.Bxf2.e3+.Qd5.Bxd5+.cxd5.exf2.Rf1.Rf8.axb5.Qg4.Kg2.Qf3+.Kh3.Rf5.Ra4.Rh5+.Rh4.Rxh4+.Kxh4.Qe2.0-1", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1324956")]
        [DataRow("d4.Nf6.c4.e6.Nf3.d5.g3.Bb4+.Bd2.Be7.Bg2.O-O.O-O.c6.Bf4.b6.Nc3.Ba6.cxd5.cxd5.Rc1.Nc6.Nxd5.Qxd5.Ne5.Nxd4.Bxd5.Nxe2+.Qxe2.Bxe2.Bxa8.Rxa8.Rfe1.Bb5.Rc2.Nd5.Rec1.Bc5.Bd2.f6.b4.Bf8.Ng4.Rd8.Rc8.Rd7.Nh6+.gxh6.Bxh6.Rf7.Rd8.Ne7.Rc7.Ng6.Rcc8.e5.f4.Bd7.Ra8.Bh3.Kf2.b5.Rdb8.exf4.gxf4.Bd7.h4.Bc6.h5.Bxa8.hxg6.hxg6.Rxa8.f5.Kg3.a6.Kh4.Rg7.Kg5.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1338702")]
        [DataRow("e4.c5.Nf3.d6.d4.cxd4.Nxd4.Nf6.Nc3.a6.Be3.Ng4.Bg5.h6.Bh4.g5.Bg3.Bg7.h3.Ne5.Nf5.Bxf5.exf5.Nbc6.Nd5.e6.fxe6.fxe6.Ne3.O-O.Be2.Qe7.O-O.Rad8.Bh5.Kh8.Re1.d5.a4.Nc4.Nxc4.dxc4.Qg4.Qb4.Qxe6.Rd2.Rad1.Nd4.Qe4.Nf5.Be5.Rxf2.Bf3.Rd2.Bxg7+.Kxg7.Qe5+.Rf6.a5.Nh4.Qc7+.Rf7.Qe5+.Rf6.Bh5.Ng6.Bxg6.Rxd1.Rxd1.Kxg6.Qe4+.Kg7.Rd7+.Kg8.Qh7+.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1361600")]
        [DataRow("d4.Nf6.c4.e6.Nf3.b6.g3.Ba6.b3.Bb4+.Bd2.Be7.Bg2.c6.Bc3.d5.Ne5.Nfd7.Nxd7.Nxd7.Nd2.O-O.O-O.Nf6.e4.b5.exd5.exd5.Re1.Rb8.c5.Bc8.Nf3.Ne4.Rxe4.dxe4.Ne5.Qd5.Qe1.Bf5.g4.Bg6.f3.b4.fxe4.Qe6.Bb2.Bf6.Nxc6.Qxc6.e5.Qa6.exf6.Rfe8.Qf1.Qe2.Qf2.Qxg4.h3.Qg5.Bc1.Qh5.Bf4.Rbd8.c6.Be4.c7.Rc8.Re1.Qg6.Rxe4.Rxe4.d5.Rce8.d6.Re1+.Kh2.Qf5.Qg3.g6.Qg5.Qxg5.Bxg5.Rd1.Bc6.Re2+.Kg3.1-0", DisplayName = "https://www.chessgames.com/perl/chessgame?gid=1387655")]
        public void TestGame(string moveSource)
        {
            var moves = moveSource.Split('.').ToArray();
            var board = TestBoard.PopulatedDefault();

            var colorTurn = Color.White;
            int turn = 0;

            while (!board.CheckForEndOfGame())
            {
                Assert.IsTrue(turn < moves.Length, "game is still going despite there being no more moves");
                var move = moves[turn];
                Assert.IsTrue(board.TryPerformAlgebraicChessNotationMove(colorTurn, move), $"move failed to execute [{turn}: {move}]");
                colorTurn = colorTurn.Invert();
                turn += 1;
            }
            if (!moves[turn - 1].EndsWith('#'))
                Assert.IsTrue(turn == moves.Length, $"game ended prematurely [{turn} / {moves.Length}]");
        }
    }
}
