-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: May 09, 2025 at 05:20 PM
-- Server version: 8.3.0
-- PHP Version: 8.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cosmeticshop`
--
CREATE DATABASE IF NOT EXISTS `cosmeticshop` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `cosmeticshop`;

-- --------------------------------------------------------

--
-- Table structure for table `categories`
--

CREATE TABLE IF NOT EXISTS `categories` (
  `IDCategorie` int NOT NULL AUTO_INCREMENT,
  `NomCategorie` varchar(100) NOT NULL,
  PRIMARY KEY (`IDCategorie`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `categories`
--

INSERT INTO `categories` (`IDCategorie`, `NomCategorie`) VALUES
(1, 'Produit de maquillage'),
(2, 'Produit soin de la peau');

-- --------------------------------------------------------

--
-- Table structure for table `commandes`
--

CREATE TABLE IF NOT EXISTS `commandes` (
  `CommandeID` int NOT NULL AUTO_INCREMENT,
  `ProduitID` int DEFAULT NULL,
  `Quantite` int DEFAULT NULL,
  `DateCommande` date DEFAULT NULL,
  `Montant` decimal(10,2) DEFAULT NULL,
  `PayeAuVendeur` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`CommandeID`),
  KEY `ProduitID` (`ProduitID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `factures`
--

CREATE TABLE IF NOT EXISTS `factures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MontantSousTotal` decimal(10,2) DEFAULT NULL,
  `MontantTPS` decimal(10,2) DEFAULT NULL,
  `MontantTVQ` decimal(10,2) DEFAULT NULL,
  `MontantTotal` decimal(10,2) DEFAULT NULL,
  `DateCreation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `factures`
--

INSERT INTO `factures` (`Id`, `MontantSousTotal`, `MontantTPS`, `MontantTVQ`, `MontantTotal`, `DateCreation`) VALUES
(1, 91.00, 4.55, 9.08, 104.63, '2025-04-11 03:43:45'),
(2, 72.00, 3.60, 7.18, 82.78, '2025-04-11 03:43:45'),
(3, 58.00, 2.90, 5.79, 66.69, '2025-04-11 03:43:45'),
(4, 33.00, 1.65, 3.29, 37.94, '2025-04-11 03:43:45'),
(5, 100.00, 5.00, 10.00, 115.00, '2025-04-11 03:43:45'),
(6, 72.00, 3.60, 7.18, 82.78, '2025-04-11 03:43:45'),
(7, 122.00, 6.10, 12.17, 140.27, '2025-04-11 03:43:45'),
(8, 38.00, 1.90, 3.79, 43.69, '2025-04-11 03:43:45'),
(9, 72.00, 3.60, 7.18, 82.78, '2025-04-11 03:43:45'),
(10, 72.00, 3.60, 7.18, 82.78, '2025-04-11 03:43:45'),
(11, 91.00, 4.55, 9.08, 104.63, '2025-04-11 03:43:45'),
(12, 38.00, 1.90, 3.79, 43.69, '2025-04-11 03:43:45'),
(13, 58.00, 2.90, 5.79, 66.69, '2025-04-11 03:43:45'),
(14, 38.00, 1.90, 3.79, 43.69, '2025-04-11 03:43:45'),
(15, 9.00, 0.45, 0.90, 10.35, '2025-04-11 03:43:45'),
(16, 24.00, 1.20, 2.39, 27.59, '2025-04-11 03:43:45'),
(17, 96.00, 4.80, 9.58, 110.38, '2025-04-11 10:19:52'),
(18, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:09:57'),
(19, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:10:28'),
(20, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:12:56'),
(21, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:20:32'),
(22, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:23:22'),
(23, 38.00, 1.90, 3.79, 43.69, '2025-04-11 15:40:05'),
(24, 24.00, 1.20, 2.39, 27.59, '2025-04-11 15:40:18'),
(25, 62.00, 3.10, 6.18, 71.28, '2025-04-30 19:54:14'),
(26, 24.00, 1.20, 2.39, 27.59, '2025-04-30 20:06:16'),
(27, 62.00, 3.10, 6.18, 71.28, '2025-05-02 15:27:48'),
(28, 43.00, 2.15, 4.29, 49.44, '2025-05-08 22:37:37'),
(29, 137.00, 6.85, 13.67, 157.52, '2025-05-08 23:08:41'),
(30, 39.00, 1.95, 3.89, 44.84, '2025-05-09 10:34:56'),
(31, 28.00, 1.40, 2.79, 32.19, '2025-05-09 10:35:37');

-- --------------------------------------------------------

--
-- Table structure for table `paiement`
--

CREATE TABLE IF NOT EXISTS `paiement` (
  `IdPaiement` int NOT NULL AUTO_INCREMENT,
  `IdVendeur` int NOT NULL,
  `Montant` decimal(10,2) NOT NULL,
  `DatePaiement` date NOT NULL,
  PRIMARY KEY (`IdPaiement`),
  KEY `IdVendeur` (`IdVendeur`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `produit`
--

CREATE TABLE IF NOT EXISTS `produit` (
  `IdProduit` int NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Nom` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Prix` decimal(10,2) NOT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `IdCategorie` int NOT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  `Quantite` int NOT NULL DEFAULT '0',
  `IdVendeur` int DEFAULT NULL,
  PRIMARY KEY (`IdProduit`),
  UNIQUE KEY `Code` (`Code`),
  UNIQUE KEY `Code_UNIQUE` (`Code`),
  KEY `idx_IdCategorie` (`IdCategorie`),
  KEY `fk_vendeur` (`IdVendeur`)
) ENGINE=MyISAM AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `produit`
--

INSERT INTO `produit` (`IdProduit`, `Code`, `Nom`, `Prix`, `Image`, `IdCategorie`, `Description`, `Quantite`, `IdVendeur`) VALUES
(1, 'P001', 'Crème hydratante', 19.99, 'image1.jpg', 1, 'Crème hydratante pour tous types de peau.', 25, 12),
(2, 'P002', 'Shampooing nourrissant', 9.99, 'image2.jpg', 2, 'Shampooing nourrissant pour cheveux secs.', 10, 13),
(3, 'P003', 'Baume réparateur', 14.49, 'image3.jpg', 1, 'Baume réparateur pour peaux sèches.', 30, 14),
(4, 'P004', 'Gel douche revitalisant', 7.99, 'image4.jpg', 2, 'Gel douche revitalisant pour tous types de peau.', 20, 15),
(5, 'P005', 'Crème hydratante', 9.99, 'image5.jpg', 1, 'Crème hydratante pour tous types de peau.', 50, 12),
(6, 'P006', 'Shampooing nourrissant', 14.99, 'image6.jpg', 2, 'Shampooing nourrissant pour cheveux secs.', 20, 16),
(7, 'P007', 'Baume réparateur', 24.49, 'image7.jpg', 1, 'Baume réparateur pour peaux sèches.', 30, 17),
(8, 'P008', 'Gamme love', 12.00, 'image8.jpg', 2, 'Gamme pour la peau noire, hydratante, nourrissant et réparateur.', 50, 18),
(9, 'P009', 'Fond de teint', 5.49, 'image9.jpg', 1, 'Fond de teint clair.', 30, 19),
(10, 'P010', 'Sérum éclat', 29.99, 'image10.jpg', 3, 'Sérum éclat pour un teint lumineux.', 40, 20),
(11, 'P011', 'Masque hydratant', 15.49, 'image11.jpg', 1, 'Masque hydratant intense pour peau sèche.', 35, 21),
(12, 'P012', 'Lotion tonique', 12.99, 'image12.jpg', 2, 'Lotion tonique pour un nettoyage en profondeur.', 50, 12),
(13, 'P013', 'Huile nourrissante', 19.99, 'image13.jpg', 3, 'Huile nourrissante pour cheveux et peau.', 25, 13),
(14, 'P014', 'Crème anti-âge', 34.99, 'image14.jpg', 1, 'Crème anti-âge raffermissante et hydratante.', 30, 20),
(15, 'P015', 'Shampoing fortifiant', 11.99, 'image15.jpg', 2, 'Shampoing fortifiant pour cheveux fragiles.', 40, 15),
(16, 'P016', 'Gel nettoyant', 9.49, 'image16.jpg', 1, 'Gel nettoyant visage doux pour tous types de peau.', 45, 16),
(17, 'P017', 'Baume à lèvres', 5.99, 'image17.jpg', 3, 'Baume à lèvres nourrissant et réparateur.', 60, 17),
(18, 'P018', 'Masque purifiant', 17.99, 'image18.jpg', 1, 'Masque purifiant à l’argile pour peaux mixtes.', 35, 18),
(19, 'P019', 'Fond de teint fluide', 22.99, 'image19.jpg', 1, 'Fond de teint fluide couvrance naturelle.', 50, 19);

-- --------------------------------------------------------

--
-- Table structure for table `produitsfactures`
--

CREATE TABLE IF NOT EXISTS `produitsfactures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdFacture` int DEFAULT NULL,
  `IdProduit` int DEFAULT NULL,
  `PrixUnitaire` decimal(10,2) DEFAULT NULL,
  `Quantite` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdFacture` (`IdFacture`),
  KEY `IdProduit` (`IdProduit`)
) ENGINE=MyISAM AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `produitsfactures`
--

INSERT INTO `produitsfactures` (`Id`, `IdFacture`, `IdProduit`, `PrixUnitaire`, `Quantite`) VALUES
(1, 1, 3, 14.00, 1),
(2, 1, 7, 24.00, 1),
(3, 1, 14, 34.00, 1),
(4, 1, 1, 19.00, 1),
(5, 2, 7, 24.00, 1),
(6, 2, 3, 14.00, 1),
(7, 2, 14, 34.00, 1),
(8, 3, 7, 24.00, 1),
(9, 3, 14, 34.00, 1),
(10, 4, 1, 19.00, 1),
(11, 4, 5, 9.00, 1),
(12, 4, 9, 5.00, 1),
(13, 6, 7, 24.00, 1),
(14, 6, 3, 14.00, 1),
(15, 6, 14, 34.00, 1),
(16, 7, 7, 24.00, 1),
(17, 7, 3, 14.00, 7),
(18, 8, 3, 14.00, 1),
(19, 8, 7, 24.00, 1),
(20, 9, 7, 24.00, 1),
(21, 9, 3, 14.00, 1),
(22, 9, 14, 34.00, 1),
(23, 10, 7, 24.00, 1),
(24, 10, 3, 14.00, 1),
(25, 10, 14, 34.00, 1),
(26, 11, 1, 19.00, 1),
(27, 11, 7, 24.00, 1),
(28, 11, 3, 14.00, 1),
(29, 11, 14, 34.00, 1),
(30, 12, 3, 14.00, 1),
(31, 12, 7, 24.00, 1),
(32, 13, 14, 34.00, 1),
(33, 13, 1, 19.00, 1),
(34, 13, 9, 5.00, 1),
(35, 14, 1, 19.00, 2),
(36, 15, 2, 9.00, 1),
(37, 16, 7, 24.00, 1),
(38, 17, 7, 24.00, 2),
(39, 17, 3, 14.00, 1),
(40, 17, 14, 34.00, 1),
(41, 18, 3, 14.00, 1),
(42, 18, 7, 24.00, 1),
(43, 19, 7, 24.00, 1),
(44, 19, 3, 14.00, 1),
(45, 20, 3, 14.00, 1),
(46, 20, 7, 24.00, 1),
(47, 21, 3, 14.00, 1),
(48, 21, 7, 24.00, 1),
(49, 22, 7, 24.00, 1),
(50, 22, 3, 14.00, 1),
(51, 23, 7, 24.00, 1),
(52, 23, 3, 14.00, 1),
(53, 24, 7, 24.00, 1),
(54, 25, 3, 14.00, 1),
(55, 25, 7, 24.00, 2),
(56, 26, 7, 24.00, 1),
(57, 27, 3, 14.00, 1),
(58, 27, 7, 24.00, 2),
(59, 28, 7, 24.00, 1),
(60, 28, 1, 19.00, 1),
(61, 29, 3, 14.00, 2),
(62, 29, 7, 24.00, 1),
(63, 29, 14, 34.00, 1),
(64, 29, 8, 12.00, 3),
(65, 29, 11, 15.00, 1),
(66, 30, 9, 5.00, 1),
(67, 30, 19, 22.00, 1),
(68, 30, 8, 12.00, 1),
(69, 31, 1, 19.00, 1),
(70, 31, 5, 9.00, 1);

-- --------------------------------------------------------

--
-- Table structure for table `teint`
--

CREATE TABLE IF NOT EXISTS `teint` (
  `TeintID` int NOT NULL,
  `Code` varchar(10) NOT NULL,
  `TypePeau` varchar(50) NOT NULL,
  PRIMARY KEY (`TeintID`),
  UNIQUE KEY `Code` (`Code`),
  UNIQUE KEY `TypePeau` (`TypePeau`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `teint`
--

INSERT INTO `teint` (`TeintID`, `Code`, `TypePeau`) VALUES
(1, 'TC', 'Clair'),
(2, 'TN', 'Noir'),
(3, 'TB', 'Brun');

-- --------------------------------------------------------

--
-- Table structure for table `utilisateurs`
--

CREATE TABLE IF NOT EXISTS `utilisateurs` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `CodeUtilisateur` varchar(10) DEFAULT NULL,
  `NomUtilisateur` varchar(50) DEFAULT NULL,
  `MotDePasse` varchar(50) DEFAULT NULL,
  `Role` enum('Client','Vendeur','Manager') DEFAULT NULL,
  `Salt` blob,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `CodeUtilisateur` (`CodeUtilisateur`),
  UNIQUE KEY `NomUtilisateur` (`NomUtilisateur`)
) ENGINE=MyISAM AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `utilisateurs`
--

INSERT INTO `utilisateurs` (`ID`, `CodeUtilisateur`, `NomUtilisateur`, `MotDePasse`, `Role`, `Salt`) VALUES
(1, 'CLI001', 'AliceDupont', 'client1', 'Client', NULL),
(2, 'CLI002', 'JeanMartin', 'client2', 'Client', NULL),
(3, 'CLI003', 'SophieDurand', 'client3', 'Client', NULL),
(4, 'CLI004', 'MarcBernard', 'client4', 'Client', NULL),
(5, 'CLI005', 'EmmaMoreau', 'client5', 'Client', NULL),
(6, 'CLI006', 'LucasFournier', 'client6', 'Client', NULL),
(7, 'CLI007', 'ChloeGirard', 'client7', 'Client', NULL),
(8, 'CLI008', 'NoahLemoine', 'client8', 'Client', NULL),
(9, 'CLI009', 'LeaRousseau', 'client9', 'Client', NULL),
(10, 'CLI010', 'HugoBlanc', 'client10', 'Client', NULL),
(11, 'MAN001', 'SuperManager', 'admin123', 'Manager', NULL),
(12, 'VEN001', 'PaulDurand', 'vendeur1', 'Vendeur', NULL),
(13, 'VEN002', 'NadiaLeroy', 'vendeur2', 'Vendeur', NULL),
(14, 'VEN003', 'KarimBenoit', 'vendeur3', 'Vendeur', NULL),
(15, 'VEN004', 'SoniaMarchand', 'vendeur4', 'Vendeur', NULL),
(16, 'VEN005', 'JulienPerrot', 'vendeur5', 'Vendeur', NULL),
(17, 'VEN006', 'LauraDubois', 'vendeur6', 'Vendeur', NULL),
(18, 'VEN007', 'MehdiRoche', 'vendeur7', 'Vendeur', NULL),
(19, 'VEN008', 'CelineLemoine', 'vendeur8', 'Vendeur', NULL),
(20, 'VEN009', 'AntoineGiraud', 'vendeur9', 'Vendeur', NULL),
(21, 'VEN010', 'SarahBarbier', 'vendeur10', 'Vendeur', NULL),
(22, NULL, 'Mariano', 'A?&nEq', 'Client', 0x778469a44b15be79da7a663ea84c819b),
(23, NULL, 'leslie', 'Dӝ', 'Client', 0xe4ce49a2e131cc4494920c9bde2d244c),
(24, NULL, 'valdess', '1', 'Client', 0x00000000000000000000000000000000),
(25, NULL, 'audrey', ')', 'Client', 0x5c271209934ba0cc9b7e884b6a51c43e),
(26, NULL, 'loveline', '', 'Client', 0x5402b0c56366f9274c800de647ab5dfe),
(27, 'MARIE12', 'marie', '<', 'Vendeur', 0xdb78d1c01687cb31db32b8ebdae67a71);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
